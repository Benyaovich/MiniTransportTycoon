
#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;
using Model.Interfaces;

public class RoadCell : Cell, IPurchasable, IHighlightable
{
    public event EventHandler<Location>? OnHighlightEnabled;
    public event EventHandler<Location>? OnHighlightDisabled;
    public bool IsVertexPoint { get; protected set; }
    public bool IsIntersection { get; protected set; }
    public TrafficLamp? Lamp { get; private set; }
    public bool HasLamp => Lamp is not null;
    public List<Direction> Directions { get; protected set; }
    public List<Vehicle> Vehicles { get; private set; } = new();
    public int BuildPrice { get; set; }
    
    public bool Highlighted { get; protected set; }
    

    public RoadCell(Location origin, bool isIntersection, List<Direction> directions,
         bool isVertexPoint = false, Size? size = null, bool destroyable = true)
        : base(origin, size, destroyable)
    {
        IsIntersection = isIntersection;
        Directions = directions;
        IsVertexPoint = isVertexPoint;
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

    public void SetHighlighted(bool value)
    {
        Highlighted = value;
        if(Highlighted) { OnHighlightEnabled?.Invoke(this, Origin); }
        else{ OnHighlightDisabled?.Invoke(this, Origin); }
    }

    public void SetIsVertexPoint(bool value)
    {
        IsVertexPoint = value;
    }
}
