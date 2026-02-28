using NUnit.Framework;

public class ForestCellTests
{
    
    [Test]
    public void ForestCellGrowingTest()
    {
        bool isGrew = false;
        ForestCell forestCell = new ForestCell(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forestCell.OnGrow += (sender,e) => { isGrew = true; };
        
        Assert.AreEqual(forestCell.NumOfTrees,1);
        forestCell.Tick(1);
        Assert.AreEqual(forestCell.NumOfTrees,2);
        Assert.IsTrue(isGrew);
    }
    
    [Test]
    public void ForestCellNotGrowingWhenReachedMaxNumberOfTreesTest()
    {
        ForestCell forestCell = new ForestCell(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forestCell.Tick(1);
        forestCell.Tick(1);
        forestCell.Tick(1);
        Assert.AreEqual(forestCell.NumOfTrees,4);
        forestCell.Tick(1);
        Assert.AreEqual(forestCell.NumOfTrees,4);
    }
    
    [Test]
    public void ForestCellSpreadingTest()
    {
        bool isSpread = false;
        ForestCell forestCell = new ForestCell(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forestCell.OnSpread += (sender,e) => { isSpread = true; };
        forestCell.Tick(1);
        forestCell.Tick(1);
        for (int i = 0; i < 20; i++)
        {
            forestCell.Tick(1);
        }
        Assert.IsTrue(isSpread);
    }
    
    [Test]
    public void ForestCellNotSpreadingWhenNumberOfTreesIsNotEnoughTest()
    {
        bool isSpread = false;
        ForestCell forestCell = new ForestCell(new Location(0, 0),growthInterval:1,spreadInterval:1);
        forestCell.OnSpread += (sender,e) => { isSpread = true; };
        Assert.IsFalse(isSpread);
        forestCell.Tick(1);
        Assert.IsFalse(isSpread);
    }
}
