using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyshareDL.Entities;
using KeyShareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly KeyShareContext _context;

        public VehicleRepository(KeyShareContext context)
        {
            _context = context;
        }

        public Vehicle? GetVehicleById(int id)
        {
            return _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.Images)
                .Include(v => v.AvailabilityCalendar)
                .Include(v => v.Reviews)
                .Include(v => v.Bookings)
                .FirstOrDefault(v => v.VehicleId == id);
        }

        public List<Vehicle> GetAllVehicles()
        {
            return _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.Images)
                .Include(v => v.AvailabilityCalendar)
                .ToList();
        }

        public List<Vehicle> GetVehiclesByOwner(int ownerId)
        {
            return _context.Vehicles
                .Include(v => v.Images)
                .Include(v => v.AvailabilityCalendar)
                .Where(v => v.OwnerId == ownerId)
                .ToList();
        }

        public Vehicle AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            return vehicle;
        }

        public Vehicle UpdateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            return vehicle;
        }

        public Vehicle DeleteVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            return vehicle;
        }
    }
}