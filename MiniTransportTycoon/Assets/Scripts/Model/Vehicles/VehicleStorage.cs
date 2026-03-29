using System;
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Vehicles
{
    public class VehicleStorage : IVehicleStorage
    {
        public event EventHandler<Vehicle> OnVehicleAdd; 
        public event EventHandler<Vehicle> OnVehicleRemove;
        public List<Vehicle> Vehicles { get; private set; } = new();

        public void AddVehicle(Vehicle vehicle)
        {
            if (Vehicles.Contains(vehicle)) return;
            Vehicles.Add(vehicle);
            Vehicles = Vehicles.OrderByDescending(x => x.MoveSpeed).ToList();
            OnVehicleAdd?.Invoke(this, vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            if (!Vehicles.Contains(vehicle)) return;
            Vehicles.Remove(vehicle);
            vehicle.RemoveFromRoadCell();
            OnVehicleRemove?.Invoke(this, vehicle);
        }
    }
}