using System;
using Model.Vehicles.SemiTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "WoodSemiTruckSO", menuName = "Vehicles/SemiTrucks/WoodSemiTruckSO")]
public class WoodSemiTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new WoodSemiTruck(grid, Resource.Wood, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(WoodSemiTruck);
}
