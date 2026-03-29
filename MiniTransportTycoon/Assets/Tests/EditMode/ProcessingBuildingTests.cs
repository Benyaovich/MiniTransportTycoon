using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ProcessingBuildingTests
{
    [Test]
    public void ResourceAmountDoesntIncreaseWhenRequiredIs0()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1,1,0,1,1));
        
        Assert.AreEqual(0,b.ResourceAmount);
        b.Tick(1);
        Assert.AreEqual(0,b.ResourceAmount);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(0,b.ResourceAmount);
    }

    [Test]
    public void AddingRequiredResourceReturn0WhenAddingNegativeAndDoesntChangeTheAmount()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1,1,0,1,1));

        Assert.AreEqual(0, b.RequiredResourceAmount);
        Assert.AreEqual(0, b.AddResource(0));
        Assert.AreEqual(0, b.RequiredResourceAmount);
        Assert.AreEqual(0, b.AddResource(-1));
        Assert.AreEqual(0, b.RequiredResourceAmount);
        Assert.AreEqual(0, b.AddResource(-100));
        Assert.AreEqual(0, b.RequiredResourceAmount);
            
    }

    [Test]
    public void AddingRequiredResourceAddTheCorrectAmount()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        Assert.AreEqual(0, b.RequiredResourceAmount);
        b.AddResource(10);
        Assert.AreEqual(10, b.RequiredResourceAmount);
        b.AddResource(20);
        Assert.AreEqual(30, b.RequiredResourceAmount);
    }

    [Test]
    public void AddingRequiredResourceDoesntGoOverCapacity()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        Assert.AreEqual(0, b.RequiredResourceAmount);
        b.AddResource(50);
        Assert.AreEqual(50, b.RequiredResourceAmount);
        b.AddResource(60);
        Assert.AreEqual(100, b.RequiredResourceAmount);
        b.AddResource(60);
        Assert.AreEqual(100, b.RequiredResourceAmount);

    }

    [Test]
    public void AddingRequiredResourceReturnsTheLeftOverResources()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        Assert.AreEqual(0,b.AddResource(10));
        Assert.AreEqual(0,b.AddResource(20));
        Assert.AreEqual(0,b.AddResource(50));
        Assert.AreEqual(80,b.AddResource(100));
        Assert.AreEqual(100,b.AddResource(100));
    }

    [Test]
    public void ProdcuesResourceWhenRequiredResourceIsEnough()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        b.AddResource(1);
        Assert.AreEqual(0, b.ResourceAmount);
        b.Tick(1);
        Assert.AreEqual(1, b.ResourceAmount);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(1, b.ResourceAmount);
        b.AddResource(2);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(3, b.ResourceAmount);
    }

    [Test]
    public void RequiredResourceDecreasesWhenProducing()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        b.AddResource(5);
        Assert.AreEqual(5,b.RequiredResourceAmount);
        b.Tick(1);
        Assert.AreEqual(4,b.RequiredResourceAmount);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(2,b.RequiredResourceAmount);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(0,b.RequiredResourceAmount);
        Assert.AreEqual(5,b.ResourceAmount);
    }
}
