using System;
using Model.Vehicles.SemiTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "IronSemiTruckSO", menuName = "Vehicles/SemiTrucks/IronSemiTruckSO")]
public class IronSemiTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new IronSemiTruck(grid, Resource.Iron, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(IronSemiTruck);
}
