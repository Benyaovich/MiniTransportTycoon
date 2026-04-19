using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "PaperCargoTruckSO", menuName = "Vehicles/CargoTrucks/PaperCargoTruckSO")]
public class PaperCargoTruckSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new PaperCargoTruck(grid, Resource.Paper, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(PaperCargoTruck);
}
