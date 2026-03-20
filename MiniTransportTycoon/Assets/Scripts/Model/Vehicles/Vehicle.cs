using System;
using JetBrains.Annotations;
using Mono.Cecil;
using UnityEngine;
using System.Collections.Generic;

public abstract class Vehicle : IAdvancable
{
    public Resource Resource { get; private set;}
    public float MoveSpeed { get; private set; }
    public int MaintenanceCost { get; private set; }
    public int PurchaseCost { get; private set; }
    public int ResourceAmount { get; protected set; }
    public Route? Route { get; set; }
    public Location? CurrentLocation => Route.CurrentLocation;
    private RoadCell? _currentRoad;
    
    private int maxCapacity;
    protected int MaxCapacity => maxCapacity;
    private Timer maintenanceTimer;
    private Timer moveTimer;
    
    public event EventHandler CarMove;

    public Vehicle(Resource resource, float speed, int maintenanceCost, int purchaseCost, int maxCapacity)
    {
        this.Resource = resource;
        this.MoveSpeed = speed;
        this.MaintenanceCost = maintenanceCost;
        this.PurchaseCost = purchaseCost;
        this.maxCapacity = maxCapacity;
        ResourceAmount = 0;
        
        moveTimer = new Timer(1 / speed);
        moveTimer.OnTimerElapsed += (object sender, EventArgs e) => CarMove?.Invoke(this, EventArgs.Empty);
    }

    public void NextStep(Cell cell, List<Cell> neighbouringCells)
    {
        foreach (var outsideCells in neighbouringCells)
        {
            if (outsideCells is Facility facility)
            {
                if (facility.ProducedResource == Resource && ResourceAmount <=  MaxCapacity - 1)
                {
                    LoadResource(facility);
                    return;
                } else if (facility is ProcessingBuilding pBuilding &&  pBuilding.RequiredResource == Resource && ResourceAmount > 0)
                {
                    UnloadResource(pBuilding);
                    return;
                }
            }
        }
        
        MoveNext(cell);
    }
    
    private void MoveNext(Cell cell)
    {
        if (cell is RoadCell road && CanMove(road))
        {
            _currentRoad?.Vehicles.Remove(this);
            Route.StepVertex();
            _currentRoad = road;
            _currentRoad.AddVehicle(this);
        }
    }

    private bool CanMove(RoadCell road)
    {
        if (RightDirecion(road) && SafeToMove(road))
        {
            return true;
        }
        
        return false;
    }

    private bool SafeToMove(RoadCell road)
    {
        if (road.IsIntersection)
        {
            if (road.HasLamp) // ide vissza terni lampa implementalas utan: && road.Lamp.Passable( == true)
            {
                foreach (var observedVehicle in road.Vehicles)
                {
                    if (!(IsInterSectionPassable(observedVehicle.Route, road)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var observedVehicle in road.Vehicles)
                {
                    if (!(IsInterSectionPassable(observedVehicle.Route, road)))
                    {
                        return false;
                    }
                }
                
                return true;
            }
        }
        else
        {
            foreach (var observedVehicle in road.Vehicles)
            {
                if (observedVehicle.Route.PreviousDirection == Route.CurrentDirection)
                {
                    return false;
                }
            }
            
            return true;
        }
        return false;
    }

    private bool IsInterSectionPassable(Route oVRoute, RoadCell road)
    {
        //jobb kanyar
        if (Route.CurrentDirection.TurnRightClockwise() == Route.NextDirection)
        {
            if (oVRoute.CurrentDirection == Route.NextDirection)
            {
                return false;
            }
            
            return true;
        } //egyenesen
        else if (Route.CurrentDirection == Route.NextDirection)
        {
            //szembol jon
            if (oVRoute.PreviousDirection.Opposite() == Route.CurrentDirection && 
                (oVRoute.CurrentDirection == Route.CurrentDirection.Opposite() || oVRoute.CurrentDirection.TurnRightClockwise() == Route.CurrentDirection))
            {
                return true;
            } //balrol jon
            else if (oVRoute.PreviousDirection.TurnLeftClockwise() == Route.CurrentDirection && oVRoute.CurrentDirection == Route.CurrentDirection.Opposite())
            {
                return true;
            }
        } //bal kanyar
        else if (Route.CurrentDirection.TurnLeftClockwise() == Route.NextDirection)
        {
            if (oVRoute.PreviousDirection.TurnLeftClockwise() == Route.CurrentDirection && oVRoute.CurrentDirection == Route.CurrentDirection.Opposite())
            {
                return true;
            }
        } //vissza fordulas - ha ures a lista
        
        return false;
    }

    private bool RightDirecion(RoadCell road)
    {
        //merre van az auto az uthoz kepest (jobbra, balra, ...)
        int dx = road.Origin.X - CurrentLocation.X;
        int dy = road.Origin.Y - CurrentLocation.Y;
            
        switch (dx, dy)
        {
            case (0, 0):
                throw new ArgumentException("Ezen a cellan van a jármu");
            
            case (var x, 0) when x != 0:
                if (road.Directions.Contains(Converter.Opposite(Route.CurrentDirection)) && 
                    road.Directions.Contains(Route.NextDirection))
                {
                    return true;
                }
                break;
            
            case (0, var y) when y != 0:
                if (road.Directions.Contains(Route.CurrentDirection) && 
                    road.Directions.Contains(Converter.Opposite(Route.NextDirection)))
                {
                    return true;
                }
                break;
            default:
                break;
        }

        return false;
    }

    protected abstract void LoadResource(Facility processingBuilding);

    protected abstract void UnloadResource(ProcessingBuilding extractorBuilding);

    public void Tick(float delta)
    {
        moveTimer.Tick(delta);
    }
    
}
