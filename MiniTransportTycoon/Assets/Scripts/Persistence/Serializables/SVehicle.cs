using System;
using JetBrains.Annotations;
using Model.Vehicles.Buses;

[Serializable]
public class SVehicle
{
    [CanBeNull] public SLocation currentLocation; // nem kell
    public int resourceAmount;
    [CanBeNull] public SRoute route;
    public float maintenanceRemainingTime;
    public float? moveRemainingTime;
    
    public SVehicle(Vehicle vehicle)
    {
        if (vehicle.CurrentLocation != null)
            currentLocation = new SLocation(vehicle.CurrentLocation);
        resourceAmount = vehicle.ResourceAmount;
        if (vehicle.Route != null)
            route = new SRoute(vehicle.Route);
        maintenanceRemainingTime = vehicle.MaintenanceTimer.RemainingTime;
        if (vehicle.MoveTimer != null)
            moveRemainingTime = vehicle.MoveTimer.RemainingTime;


    }
    
    public SVehicle() { }
}

[Serializable]
public class SSlowBus : SVehicle
{
    public SSlowBus(SlowBus slowBus) : base(slowBus) { }
    public SSlowBus() { }
}
