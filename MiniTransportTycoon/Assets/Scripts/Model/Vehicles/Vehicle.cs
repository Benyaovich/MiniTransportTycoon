using System;
using JetBrains.Annotations;
using Mono.Cecil;
using UnityEngine;

public class Vehicle : IAdvancable
{
    private Timer vehicleTimer; // od-ba
    public Resource Resource { get; private set;}
    public float MoveSpeed { get; private set; }
    public int MaintenanceCost { get; private set; }
    public int PurchaseCost { get; private set; }
    public int ResourceAmount { get; private set; }
    public Route? Route { get; private set; }
    public Location?  CurrentLocation { get; private set; }
    
    private int maxCapacity;
    private Timer maintenanceTimer;
    private Timer moveTimer; // szerintem nem lesz szukseg ra, will see
    private bool canMove; // eleg lesz a function
    private RoadCell roadCell; // the current cell

    public Vehicle(Resource resource, float speed, int maintenanceCost, int purchaseCost, int resourceAmount)
    {
        this.Resource = resource;
        this.MoveSpeed = speed;
        this.MaintenanceCost = maintenanceCost;
        this.PurchaseCost = purchaseCost;
        this.ResourceAmount = resourceAmount;
        
        vehicleTimer = new Timer(1 / speed);
        vehicleTimer.OnTimerElapsed += (MoveNext);
    }

    private void MoveNext(object o, EventArgs args)
    {
        if (CanMove())
        {
            
        };
    }

    private bool CanMove()
    {
        // Route osztaly kell ide
        throw new NotImplementedException();
    }

    public void Tick(float delta)
    {
        vehicleTimer.Tick(delta);
    }
}
