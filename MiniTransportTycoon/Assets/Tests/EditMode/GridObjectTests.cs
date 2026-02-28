using System.Numerics;
using NUnit.Framework;
using System.Collections.Generic;

public class GridObjectTests
{
    Grid<GridObject> grid = new Grid<GridObject>(new Size(3, 3), 10, Vector3.Zero,
        (Grid<GridObject> g, Location l) => new GridObject(g,l));

    [Test]
    public void SetValue()
    {
        GridObject g1 = new GridObject(grid, new Location(0, 0));
        Assert.IsInstanceOf<Field>(g1.Value);
        Cell cell = new Cell(new Location(0, 0));
        g1.SetValue(cell);
        Assert.AreEqual(cell, g1.Value);
    }

    [Test]
    public void SettingValueInvokesGridObjectChanged()
    {
        GridObject g1 = new GridObject(grid, new Location(0, 0));
   
        Cell cell = new Cell(new Location(0, 0));

        bool invoked = false;
        grid.OnGridObjectChanged += (sender, args) => invoked = true;
        Assert.IsFalse(invoked);
        g1.SetValue(cell);
        Assert.IsTrue(invoked);
        
    }

    [Test]
    public void ClearValue()
    {
        GridObject g1 = new GridObject(grid, new Location(0, 0));
        Cell cell = new Cell(new Location(0, 0));
        g1.SetValue(cell);
        
        g1.ClearValue();
        
        Assert.IsNull(g1.Value);
    }

    [Test]
    public void ClearValueInvokesGridObjectChanged()
    {
        GridObject g1 = new GridObject(grid, new Location(0, 0));
   
        Cell cell = new Cell(new Location(0, 0));

        bool invoked = false;
        g1.SetValue(cell);
        
        grid.OnGridObjectChanged += (sender, args) => invoked = true;
        
        g1.ClearValue();
        
        Assert.IsTrue(invoked);
    }
    

}
