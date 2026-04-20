using System;
using Model.Vehicles.SemiTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "PaperSemiTruckSO", menuName = "Vehicles/SemiTrucks/PaperSemiTruckSO")]
public class PaperSemiTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new PaperSemiTruck(grid, Resource.Paper, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(PaperSemiTruck);
}
