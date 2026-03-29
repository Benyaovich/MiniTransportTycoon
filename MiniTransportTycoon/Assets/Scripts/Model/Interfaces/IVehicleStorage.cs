using System;
using System.Collections.Generic;

namespace Model.Interfaces
{
    public interface IVehicleStorage
    {
        public event EventHandler<Vehicle> OnVehicleAdd; 
        public event EventHandler<Vehicle> OnVehicleRemove; 
        public List<Vehicle> Vehicles { get; }
        public void AddVehicle(Vehicle vehicle);
        public void RemoveVehicle(Vehicle vehicle);
    }
}