using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.VehicleDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface IVehicleServices
    {
        BasicVehicleDTO GetVehicle(int id);
        List<BasicVehicleDTO> GetAllVehicles();
        List<BasicVehicleDTO> GetVehiclesByOwner(int ownerId);
        List<BasicVehicleDTO> SearchVehicles(SearchVehicleDTO filter);
        BasicVehicleDTO CreateVehicle(int ownerId, CreateVehicleDTO dto);
        BasicVehicleDTO UpdateVehicle(int vehicleId, UpdateVehicleDTO dto);
        BasicVehicleDTO DeleteVehicle(int vehicleId);
        BasicVehicleDTO PublishVehicle(int vehicleId);
        VehicleAvailabilityDTO CheckAvailability(int vehicleId, DateOnly startDate, DateOnly endDate);
        BasicVehicleDTO AddImage(int vehicleId, UploadImageDTO dto);
    }
}
