using NUnit.Framework;

public class ExtractorBuildingTests
{
    
    [Test]
    public void ResourceAmountIsZeroAtStart()
    {
        ExtractorBuilding b = new ExtractorBuilding(
            Resource.Coal, 100,
            new Location(0, 0));
        
        Assert.AreEqual(0, b.ResourceAmount);
    }

    [Test]
    public void ResourceAmountIncreaseOverTime()
    {
        ExtractorBuilding b = new ExtractorBuilding(
            Resource.Coal, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1,1,0,1,1));
        
        b.Tick(1);
        Assert.AreEqual(1,b.ResourceAmount);
        b.Tick(1);
        Assert.AreEqual(2,b.ResourceAmount);
    }
    
    [Test]
    public void ResourceAmountDoesntGetAboveCapacity()
    {
        ExtractorBuilding b = new ExtractorBuilding(
            Resource.Coal, 1,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1,1,0,1,1));
        
        b.Tick(1);
        Assert.AreEqual(1,b.ResourceAmount);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        Assert.AreEqual(1,b.ResourceAmount);
    }

    [Test]
    public void GetProducedResource()
    {
        ExtractorBuilding b = new ExtractorBuilding(
            Resource.Coal, 100,
            new Location(0, 0),
            prodInterval: 1,
            rch: new RateChangeHandler(1,1,0,1,1));

        Assert.AreEqual(0, b.GetResource(-1));
        Assert.AreEqual(0, b.GetResource(0));
        Assert.AreEqual(0, b.GetResource(-1000));
        
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        
        Assert.AreEqual(1, b.GetResource(1));
        Assert.AreEqual(2, b.ResourceAmount);
        
        b.Tick(1);
        b.Tick(1);
        
        Assert.AreEqual(4,b.GetResource(4));
        Assert.AreEqual(0, b.ResourceAmount);
        
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        b.Tick(1);
        
        Assert.AreEqual(5, b.GetResource(100));
        Assert.AreEqual(0, b.ResourceAmount);
        
    }
}
