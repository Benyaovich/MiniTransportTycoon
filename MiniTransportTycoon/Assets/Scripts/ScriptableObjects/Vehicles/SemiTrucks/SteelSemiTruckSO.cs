using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "SteelSemiTruckSO", menuName = "Vehicles/SemiTrucks/SteelSemiTruckSO")]
public class SteelSemiTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new SteelSemiTruck(grid, Resource.Steel, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(SteelSemiTruck);
}
