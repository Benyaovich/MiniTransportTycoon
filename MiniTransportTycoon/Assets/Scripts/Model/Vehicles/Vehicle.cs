using System;
using JetBrains.Annotations;
using Mono.Cecil;
using UnityEngine;

public class Vehicle : IAdvancable
{
    public Resource Resource { get; private set;}
    public float MoveSpeed { get; private set; }
    public int MaintenanceCost { get; private set; }
    public int PurchaseCost { get; private set; }
    public int ResourceAmount { get; private set; }
    public Route? Route
    {
        get => Route;
        set
        {
            Route = value;
            CurrentLocation = Route.CurrentLocation;
        }
    }
    public Location? CurrentLocation { get => Route.CurrentLocation; private set{} }
    
    private int maxCapacity;
    private Timer maintenanceTimer;
    private Timer moveTimer; // szerintem nem lesz szukseg ra, will see
    
    public event EventHandler CarMove;

    public Vehicle(Resource resource, float speed, int maintenanceCost, int purchaseCost, int resourceAmount)
    {
        this.Resource = resource;
        this.MoveSpeed = speed;
        this.MaintenanceCost = maintenanceCost;
        this.PurchaseCost = purchaseCost;
        this.ResourceAmount = resourceAmount;
        
        moveTimer = new Timer(1 / speed);
        moveTimer.OnTimerElapsed += (object sender, EventArgs e) => CarMove?.Invoke(this, EventArgs.Empty);
    }

    private void MoveNext(Cell cell)
    {
        if (CanMove(cell))
        {
            Route.StepVertex();
            CurrentLocation = Route.CurrentLocation;
        };
    }

    private bool CanMove(Cell cell)
    {
        if (cell is RoadCell road)
        {
            if (RightDirecion(road))
            {
                //folyt kov
            }
        }
        
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

    public void Tick(float delta)
    {
        moveTimer.Tick(delta);
    }
    
}
