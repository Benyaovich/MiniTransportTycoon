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
        Assert.AreEqual(0, b.AddRequiredResource(0));
        Assert.AreEqual(0, b.RequiredResourceAmount);
        Assert.AreEqual(0, b.AddRequiredResource(-1));
        Assert.AreEqual(0, b.RequiredResourceAmount);
        Assert.AreEqual(0, b.AddRequiredResource(-100));
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
        b.AddRequiredResource(10);
        Assert.AreEqual(10, b.RequiredResourceAmount);
        b.AddRequiredResource(20);
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
        b.AddRequiredResource(50);
        Assert.AreEqual(50, b.RequiredResourceAmount);
        b.AddRequiredResource(60);
        Assert.AreEqual(100, b.RequiredResourceAmount);
        b.AddRequiredResource(60);
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

        Assert.AreEqual(0,b.AddRequiredResource(10));
        Assert.AreEqual(0,b.AddRequiredResource(20));
        Assert.AreEqual(0,b.AddRequiredResource(50));
        Assert.AreEqual(80,b.AddRequiredResource(100));
        Assert.AreEqual(100,b.AddRequiredResource(100));
    }

    [Test]
    public void ProdcuesResourceWhenRequiredResourceIsEnough()
    {
        ProcessingBuilding b = new ProcessingBuilding(
            Resource.Steel, Resource.Iron, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1, 1, 0, 1, 1));

        b.AddRequiredResource(1);
        Assert.AreEqual(0, b.ResourceAmount);
        b.Tick(1);
        Assert.AreEqual(1, b.ResourceAmount);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(1, b.ResourceAmount);
        b.AddRequiredResource(2);
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

        b.AddRequiredResource(5);
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
