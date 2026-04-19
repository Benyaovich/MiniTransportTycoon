
#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using Model.Enumerations;
using Model.Interfaces;

public class RoadCell : Cell, IPurchasable, IHighlightable, IDestroyable
{
    public event EventHandler<Location>? OnHighlightEnabled;
    public event EventHandler<Location>? OnHighlightDisabled;
    public bool IsVertexPoint { get; protected set; }
    public bool IsIntersection { get; protected set; }
    public TrafficLamp? Lamp { get; private set; }
    public List<Direction> Directions { get; protected set; }
    public List<Vehicle> Vehicles { get; private set; } = new(); // ami rajta vannak eppen
    public List<Vehicle> WaitingVehicles { get; private set; } = new(); // amik fel beakarnak hajtani
    public int BuildPrice { get; set; }
    
    public bool Highlighted { get; protected set; }

    public bool CanDestroy => Vehicles.Count == 0 && WaitingVehicles.Count == 0;

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
        
        if (vehicle.Route != null && vehicle.Grid.GetGridObject(vehicle.Route.NextPosition)?.Model is RoadCell nextRoadCell)
        {
            nextRoadCell.RemoveWaitingVehicle(vehicle);
        }
    }

    public void AddWaitingVehicle(Vehicle vehicle)
    {
        if (WaitingVehicles.Contains(vehicle)) return;
        WaitingVehicles.Add(vehicle);
    }

    public void RemoveWaitingVehicle(Vehicle vehicle)
    {
        if (!WaitingVehicles.Contains(vehicle)) return;
        WaitingVehicles.Remove(vehicle);
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

    public void AddTrafficLamp(TrafficLamp lamp)
    {
        Lamp = lamp;
    }

    public bool IsVehicleAllowedToPass(Vehicle tryingVehicle)
    {
        
        if(WaitingVehicles[0].Route is null) throw new NullReferenceException("The WaitingVehicles[0].Route is null");
        if(tryingVehicle.Route is null || !WaitingVehicles.Contains(tryingVehicle)) return false;
        
        if (!IsIntersection)
        {
            foreach (var observedVehicle in Vehicles)
            {
                if (observedVehicle == tryingVehicle) continue;
                if (observedVehicle.Route!.CurrentDirection.Opposite() != tryingVehicle.Route!.CurrentDirection)
                {
                    return false;
                }
            }
        }
        else
        {
            if (Lamp is not null && Lamp.IsLightOn)
            {
                if (!Lamp.PassingDirection.Contains(tryingVehicle.Route.NextDirection.Opposite())) return false;
                
                foreach (var observedVehicle in Vehicles)
                {
                    if (observedVehicle == tryingVehicle) continue;
                    if (!IsInterSectionPassable(tryingVehicle.Route!, observedVehicle.Route!))
                    { 
                        return false;
                    }
                }
            }
            else
            {
                foreach (var observedVehicle in Vehicles)
                {
                    if (observedVehicle == tryingVehicle) continue;
                    if (!IsInterSectionPassable(tryingVehicle.Route!, observedVehicle.Route!))
                    { 
                        return false;
                    }
                }
                
                if(Vehicles.Contains(tryingVehicle)) return true;
                
                if (tryingVehicle != WaitingVehicles[0] && !IsInterSectionPassable(tryingVehicle.Route!, WaitingVehicles[0].Route!))
                { 
                    return false;
                }
            }
            
            
        }
        
        return true;
    }
    
    private bool IsInterSectionPassable(Route tryingVehicleRoute, Route otherVehiclesRoute)
    {
        //jobb kanyar
        if (tryingVehicleRoute.CurrentDirection.TurnRightClockwise() == tryingVehicleRoute.NextDirection)
        {
            if (otherVehiclesRoute.CurrentDirection == tryingVehicleRoute.NextDirection)
            {
                return false;
            }
            
            return true;
        } 
        
        //egyenesen
        if (tryingVehicleRoute.CurrentDirection == tryingVehicleRoute.NextDirection)
        {
            //szembol jon
            if (otherVehiclesRoute.PreviousDirection.Opposite() == tryingVehicleRoute.CurrentDirection && 
                (otherVehiclesRoute.CurrentDirection == tryingVehicleRoute.CurrentDirection.Opposite() || otherVehiclesRoute.CurrentDirection.TurnRightClockwise() == tryingVehicleRoute.CurrentDirection))
            {
                return true;
            } //balrol jon
            else if (otherVehiclesRoute.PreviousDirection.TurnLeftClockwise() == tryingVehicleRoute.CurrentDirection && otherVehiclesRoute.CurrentDirection == tryingVehicleRoute.CurrentDirection.Opposite())
            {
                return true;
            }
        } //bal kanyar
        else if (tryingVehicleRoute.CurrentDirection.TurnLeftClockwise() == tryingVehicleRoute.NextDirection)
        {
            if (otherVehiclesRoute.PreviousDirection.TurnLeftClockwise() == tryingVehicleRoute.CurrentDirection && otherVehiclesRoute.CurrentDirection == tryingVehicleRoute.CurrentDirection.Opposite())
            {
                return true;
            }
        } //vissza fordulas - ha ures a lista
        
        return false;
    }
}
