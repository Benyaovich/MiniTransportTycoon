
#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;

public class RoadCell : Cell, IPurchasable
{
    public bool IsVertexPoint { get; private set; }
    public bool IsIntersection { get; private set; }
    public TrafficLamp? Lamp { get; private set; }
    public bool HasLamp => Lamp is not null;
    public List<Direction> Directions { get; private set; }
    public List<Vehicle> Vehicles { get; private set; } = new();
    public int BuildPrice { get; set; }


    public RoadCell(Location origin, bool isIntersection, List<Direction> directions,
        Size? size = null, bool destroyable = true)
        : base(origin, size, destroyable)
    {
        IsIntersection = isIntersection;
        Directions = directions;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        if (Vehicles.Contains(vehicle)) return;
        Vehicles.Add(vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        if (!Vehicles.Contains(vehicle)) return;
        Vehicles.Remove(vehicle);
    }

    public void AddLamp(TrafficLamp lamp)
    {
        Lamp = lamp;
    }

    public void Destroy()
    {
        // Gráfból ki kell szedni a csucsot meg hasonlok
        throw new NotImplementedException();
    }
}
