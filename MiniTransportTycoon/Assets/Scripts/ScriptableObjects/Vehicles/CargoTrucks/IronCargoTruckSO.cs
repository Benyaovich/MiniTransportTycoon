using System;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;

[CreateAssetMenu(fileName = "IronCargoTruckSO", menuName = "Vehicles/CargoTrucks/IronCargoTruckSO")]
public class IronCargoTruckSO : VehicleSO
{
    public override Vehicle Create()
    {
        return new IronCargoTruck(Resource.Iron, speed, maintenanceCost, price, maxCarryCapacity);
    }

    public override Type VehicleType => typeof(IronCargoTruck);
}
