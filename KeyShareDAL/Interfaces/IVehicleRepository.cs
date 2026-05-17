using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Entities;


namespace KeyShareDAL.Interfaces
{
    public interface IVehicleRepository
    {
        Vehicle? GetVehicleById(int id);
        List<Vehicle> GetAllVehicles();
        List<Vehicle> GetVehiclesByOwner(int ownerId);
        Vehicle AddVehicle(Vehicle vehicle);
        Vehicle UpdateVehicle(Vehicle vehicle);
        Vehicle DeleteVehicle(Vehicle vehicle);
    }
}
