using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "CoalSemiTruckSO", menuName = "Vehicles/SemiTrucks/CoalSemiTruckSO")]
public class CoalSemiTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new CoalSemiTruck(grid, Resource.Coal, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(CoalSemiTruck);
}
