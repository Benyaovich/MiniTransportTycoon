using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "SteelCargoTruckSO", menuName = "Vehicles/CargoTrucks/SteelCargoTruckSO")]
public class SteelCargoTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new SteelCargoTruck(grid, Resource.Steel, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(SteelCargoTruck);
}
