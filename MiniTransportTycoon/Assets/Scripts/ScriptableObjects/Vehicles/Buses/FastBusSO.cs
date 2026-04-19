using System;
using Model.Vehicles.Buses;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "FastBusSO", menuName = "Vehicles/Buses/FastBusSO")]
public class FastBusSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new FastBus(grid, Resource.People, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(FastBus);
}
