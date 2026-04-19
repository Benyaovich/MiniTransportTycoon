using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "CoalCargoTruckSO", menuName = "Vehicles/CargoTrucks/CoalCargoTruckSO")]
public class CoalCargoTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new CoalCargoTruck(grid, Resource.Coal, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(CoalCargoTruck);
}
