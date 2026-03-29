using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "IronCargoTruckSO", menuName = "Vehicles/CargoTrucks/IronCargoTruckSO")]
public class IronCargoTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new IronCargoTruck(grid, Resource.Iron, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(IronCargoTruck);
}
