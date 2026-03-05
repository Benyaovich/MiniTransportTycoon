using NUnit.Framework;

public class ForestCellTests
{
    
    [Test]
    public void ForestCellGrowingTest()
    {
        bool isGrew = false;
        Forest forest = new Forest(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forest.OnGrow += (sender,e) => { isGrew = true; };
        
        Assert.AreEqual(forest.NumOfTrees,1);
        forest.Tick(1);
        Assert.AreEqual(forest.NumOfTrees,2);
        Assert.IsTrue(isGrew);
    }
    
    [Test]
    public void ForestCellNotGrowingWhenReachedMaxNumberOfTreesTest()
    {
        Forest forest = new Forest(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forest.Tick(1);
        forest.Tick(1);
        forest.Tick(1);
        Assert.AreEqual(forest.NumOfTrees,4);
        forest.Tick(1);
        Assert.AreEqual(forest.NumOfTrees,4);
    }
    
    [Test]
    public void ForestCellSpreadingTest()
    {
        bool isSpread = false;
        Forest forest = new Forest(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forest.OnSpread += (sender,e) => { isSpread = true; };
        forest.Tick(1);
        forest.Tick(1);
        for (int i = 0; i < 20; i++)
        {
            forest.Tick(1);
        }
        Assert.IsTrue(isSpread);
    }
    
    [Test]
    public void ForestCellNotSpreadingWhenNumberOfTreesIsNotEnoughTest()
    {
        bool isSpread = false;
        Forest forest = new Forest(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forest.OnSpread += (sender,e) => { isSpread = true; };
        Assert.IsFalse(isSpread);
        forest.Tick(1);
        Assert.IsFalse(isSpread);
    }
}
