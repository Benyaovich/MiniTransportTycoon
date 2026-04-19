using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "WoodCargoTruckSO", menuName = "Vehicles/CargoTrucks/WoodCargoTruckSO")]
public class WoodCargoTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new WoodCargoTruck(grid, Resource.Wood, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(WoodCargoTruck);
}
