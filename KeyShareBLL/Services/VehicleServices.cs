using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyshareDL.Entities;
using KeyshareDL.Enums;
using KeyShareDL.Entities;
using KeyShareDL.Enums;
using KeySharePL.DTOs.VehicleDTOs;

namespace KeyShareBLL.Services
{
    public class VehicleServices : IVehicleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BasicVehicleDTO GetVehicle(int id)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(id);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }

        public List<BasicVehicleDTO> GetAllVehicles()
        {
            var vehicles = _unitOfWork.Vehicles.GetAllVehicles();
            if (vehicles == null || vehicles.Count == 0)
                throw new KeyNotFoundException("No vehicles found.");

            return _mapper.Map<List<BasicVehicleDTO>>(vehicles);
        }

        public List<BasicVehicleDTO> GetVehiclesByOwner(int ownerId)
        {
            var owner = _unitOfWork.Users.GetUserById(ownerId);
            if (owner == null)
                throw new KeyNotFoundException($"Owner with ID {ownerId} not found.");

            var vehicles = _unitOfWork.Vehicles.GetVehiclesByOwner(ownerId);
            return _mapper.Map<List<BasicVehicleDTO>>(vehicles);
        }

        public List<BasicVehicleDTO> SearchVehicles(SearchVehicleDTO filter)
        {
            var vehicles = _unitOfWork.Vehicles.GetAllVehicles()
                .Where(v => v.ListingStatus == VehicleListingStatus.Active
                         && v.AvailabilityStatus == VehicleStatus.Available);

            if (!string.IsNullOrWhiteSpace(filter.Location))
                vehicles = vehicles.Where(v =>
                    v.Location.Contains(filter.Location, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Type))
                vehicles = vehicles.Where(v =>
                    v.Type.Equals(filter.Type, StringComparison.OrdinalIgnoreCase));

            if (filter.MaxPricePerDay.HasValue)
                vehicles = vehicles.Where(v => v.PricePerDay <= filter.MaxPricePerDay.Value);

            if (filter.StartDate.HasValue && filter.EndDate.HasValue)
            {
                vehicles = vehicles.Where(v =>
                    !v.Bookings.Any(b =>
                        b.Status != BookingStatus.Cancelled &&
                        b.Status != BookingStatus.Rejected &&
                        b.StartDate <= filter.EndDate.Value &&
                        b.EndDate >= filter.StartDate.Value));
            }

            var result = vehicles.ToList();
            if (result.Count == 0)
                throw new KeyNotFoundException("No vehicles match the search criteria.");

            return _mapper.Map<List<BasicVehicleDTO>>(result);
        }

        public BasicVehicleDTO CreateVehicle(int ownerId, CreateVehicleDTO dto)
        {
            var owner = _unitOfWork.Users.GetUserById(ownerId);
            if (owner == null)
                throw new KeyNotFoundException($"Owner with ID {ownerId} not found.");

            if (owner.Role != UserRole.Owner && owner.Role != UserRole.Business && owner.Role != UserRole.Admin)
                throw new InvalidOperationException("Only vehicle owners or business users can list vehicles.");

            if (string.IsNullOrWhiteSpace(dto.Model))
                throw new ArgumentException("Vehicle model is required.");

            if (string.IsNullOrWhiteSpace(dto.Location))
                throw new ArgumentException("Vehicle location is required.");

            if (dto.PricePerDay <= 0)
                throw new ArgumentException("Price per day must be greater than zero.");

            var vehicle = _mapper.Map<Vehicle>(dto);
            vehicle.OwnerId = ownerId;
            vehicle.ListingStatus = VehicleListingStatus.Draft;
            vehicle.AvailabilityStatus = VehicleStatus.Available;

            _unitOfWork.Vehicles.AddVehicle(vehicle);
            _unitOfWork.Save();

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }

        public BasicVehicleDTO UpdateVehicle(int vehicleId, UpdateVehicleDTO dto)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            bool hasChanges = false;

            if (!string.IsNullOrWhiteSpace(dto.Model) && dto.Model != vehicle.Model)
            {
                vehicle.Model = dto.Model;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Type) && dto.Type != vehicle.Type)
            {
                vehicle.Type = dto.Type;
                hasChanges = true;
            }

            if (dto.PricePerDay.HasValue && dto.PricePerDay.Value != vehicle.PricePerDay)
            {
                if (dto.PricePerDay.Value <= 0)
                    throw new ArgumentException("Price per day must be greater than zero.");

                vehicle.PricePerDay = dto.PricePerDay.Value;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Location) && dto.Location != vehicle.Location)
            {
                vehicle.Location = dto.Location;
                hasChanges = true;
            }

            if (dto.Description != null && dto.Description != vehicle.Description)
            {
                vehicle.Description = dto.Description;
                hasChanges = true;
            }

            if (dto.AvailabilityStatus.HasValue && dto.AvailabilityStatus.Value != vehicle.AvailabilityStatus)
            {
                vehicle.AvailabilityStatus = dto.AvailabilityStatus.Value;
                hasChanges = true;
            }

            if (!hasChanges)
                throw new ArgumentException("Nothing to update.");

            _unitOfWork.Vehicles.UpdateVehicle(vehicle);
            _unitOfWork.Save();

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }

        public BasicVehicleDTO DeleteVehicle(int vehicleId)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            if (vehicle.Bookings.Any(b =>
                b.Status == BookingStatus.Confirmed ||
                b.Status == BookingStatus.InProgress))
                throw new InvalidOperationException("Cannot delete a vehicle with active bookings.");

            vehicle.ListingStatus = VehicleListingStatus.Deleted;
            _unitOfWork.Vehicles.UpdateVehicle(vehicle);
            _unitOfWork.Save();

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }

        public BasicVehicleDTO PublishVehicle(int vehicleId)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            if (vehicle.ListingStatus == VehicleListingStatus.Active)
                throw new InvalidOperationException("Vehicle is already published.");

            if (vehicle.ListingStatus == VehicleListingStatus.Deleted ||
                vehicle.ListingStatus == VehicleListingStatus.Removed)
                throw new InvalidOperationException("Cannot publish a deleted or removed vehicle.");

            vehicle.ListingStatus = VehicleListingStatus.Active;
            _unitOfWork.Vehicles.UpdateVehicle(vehicle);
            _unitOfWork.Save();

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }

        public VehicleAvailabilityDTO CheckAvailability(int vehicleId, DateOnly startDate, DateOnly endDate)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");

            if (startDate < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Start date cannot be in the past.");

            var hasConflict = vehicle.Bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Rejected &&
                b.StartDate <= endDate &&
                b.EndDate >= startDate);

            var calendar = vehicle.AvailabilityCalendar;
            var blockedList = new List<string>();

            if (calendar != null && !string.IsNullOrWhiteSpace(calendar.BlockedDates))
                blockedList = calendar.BlockedDates.Split(',').ToList();

            return new VehicleAvailabilityDTO
            {
                VehicleId = vehicleId,
                IsAvailable = !hasConflict && vehicle.AvailabilityStatus == VehicleStatus.Available,
                AvailableFrom = calendar?.AvailableFrom,
                AvailableTo = calendar?.AvailableTo,
                BlockedDates = blockedList
            };
        }

        public BasicVehicleDTO AddImage(int vehicleId, UploadImageDTO dto)
        {
            var vehicle = _unitOfWork.Vehicles.GetVehicleById(vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");

            if (string.IsNullOrWhiteSpace(dto.ImageUrl))
                throw new ArgumentException("Image URL is required.");

            var image = new VehicleImage
            {
                ImageUrl = dto.ImageUrl,
                VehicleId = vehicleId,
                UploadedAt = DateTime.UtcNow
            };

            vehicle.Images.Add(image);
            _unitOfWork.Vehicles.UpdateVehicle(vehicle);
            _unitOfWork.Save();

            return _mapper.Map<BasicVehicleDTO>(vehicle);
        }
    }
}