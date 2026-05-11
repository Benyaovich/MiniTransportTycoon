using System;
using JetBrains.Annotations;
using Model.Vehicles.Buses;
using Model.Vehicles.CargoTrucks;
using Model.Vehicles.SemiTrucks;

[Serializable]
public class SVehicle
{
    public int identifier;
    public int resourceAmount;
    [CanBeNull] public SRoute route;
    public float maintenanceRemainingTime;
    public float? moveRemainingTime;
    
    public SVehicle(Vehicle vehicle)
    {
        identifier = vehicle.Identifier;
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

[Serializable]
public class SFastBus : SVehicle
{
    public SFastBus(FastBus slowBus) : base(slowBus) { }
    public SFastBus() { }
}

[Serializable]
public class SCoalCargoTruck : SVehicle
{
    public SCoalCargoTruck(CoalCargoTruck coalCargoTruck) : base(coalCargoTruck) { }
    public SCoalCargoTruck() { }
}

[Serializable]
public class SIronCargoTruck : SVehicle
{
    public SIronCargoTruck(IronCargoTruck ironCargoTruck) : base(ironCargoTruck) { }
    public SIronCargoTruck() { }
}

[Serializable]
public class SPaperCargoTruck : SVehicle
{
    public SPaperCargoTruck(PaperCargoTruck paperCargoTruck) : base(paperCargoTruck) { }
    public SPaperCargoTruck() { }
}

[Serializable]
public class SSteelCargoTruck : SVehicle
{
    public SSteelCargoTruck(SteelCargoTruck steelCargoTruck) : base(steelCargoTruck) { }
    public SSteelCargoTruck() { }
}

[Serializable]
public class SWoodCargoTruck : SVehicle
{
    public SWoodCargoTruck(WoodCargoTruck woodCargoTruck) : base(woodCargoTruck) { }
    public SWoodCargoTruck() { }
}

[Serializable]
public class SCoalSemiTruck : SVehicle
{
    public SCoalSemiTruck(CoalSemiTruck coalSemiTruck) : base(coalSemiTruck) { }
    public SCoalSemiTruck() { }
}

[Serializable]
public class SIronSemiTruck : SVehicle
{
    public SIronSemiTruck(IronSemiTruck ironSemiTruck) : base(ironSemiTruck) { }
    public SIronSemiTruck() { }
}

[Serializable]
public class SPaperSemiTruck : SVehicle
{
    public SPaperSemiTruck(PaperSemiTruck paperSemiTruck) : base(paperSemiTruck) { }
    public SPaperSemiTruck() { }
}

[Serializable]
public class SSteelSemiTruck : SVehicle
{
    public SSteelSemiTruck(SteelSemiTruck steelSemiTruck) : base(steelSemiTruck) { }
    public SSteelSemiTruck() { }
}

[Serializable]
public class SWoodSemiTruck : SVehicle
{
    public SWoodSemiTruck(WoodSemiTruck woodSemiTruck) : base(woodSemiTruck) { }
    public SWoodSemiTruck() { }
}


