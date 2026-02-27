using System.Collections.Generic;
using NUnit.Framework;

public class CellTests
{
    
    [Test]
    public void GetGridPositionListWorks()
    {
        Cell c1 = new Cell(new Location(0, 0));
        List<Location> poslist = c1.GetGridPositionList();
        
        Assert.AreEqual(1, poslist.Count);
        Assert.AreEqual(0, poslist[0].X);
        Assert.AreEqual(0, poslist[0].Y);
        
        Cell c2 = new Cell(new Location(0, 0));
        poslist = c2.GetGridPositionList();
        
        Assert.AreEqual(9, poslist.Count);
        
        Assert.AreEqual(0, poslist[0].X);
        Assert.AreEqual(0, poslist[0].Y);
        
        Assert.AreEqual(1, poslist[1].X);
        Assert.AreEqual(0, poslist[1].Y);
        
        Assert.AreEqual(2, poslist[2].X);
        Assert.AreEqual(0, poslist[2].Y);
        
        Assert.AreEqual(0, poslist[3].X);
        Assert.AreEqual(1, poslist[3].Y);
        
        Assert.AreEqual(1, poslist[4].X);
        Assert.AreEqual(1, poslist[4].Y);
        
        Assert.AreEqual(2, poslist[5].X);
        Assert.AreEqual(1, poslist[5].Y);
        
        Assert.AreEqual(0, poslist[6].X);
        Assert.AreEqual(2, poslist[6].Y);
        
        Assert.AreEqual(1, poslist[7].X);
        Assert.AreEqual(2, poslist[7].Y);
        
        Assert.AreEqual(2, poslist[8].X);
        Assert.AreEqual(2, poslist[8].Y);
    
    }

}
