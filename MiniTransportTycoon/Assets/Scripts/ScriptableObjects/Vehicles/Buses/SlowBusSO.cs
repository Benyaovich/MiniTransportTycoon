using System;
using Model.Vehicles.Buses;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowBusSO", menuName = "Vehicles/Buses/SlowBusSO")]
public class SlowBusSO : VehicleSO
{
    public override Vehicle Create(Grid<ModelGridObject> grid)
    {
        return new SlowBus(grid, Resource.People, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(SlowBus);
}
