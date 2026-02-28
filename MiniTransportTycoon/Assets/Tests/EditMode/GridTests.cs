using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Vector3 = System.Numerics.Vector3;

public class GridTests
{
    private class IntGridObject
    {
        public Grid<IntGridObject> Grid;
        public Location Location;
        public int Value;

        public IntGridObject(Grid<IntGridObject> grid, Location loc)
        {
            Grid = grid;
            Location = loc;
        }
    }
    
    [Test]
    public void GridIsInitializedCorrectlyWithTheGridObjects()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));
        for (int i = 0; i < grid.Size.Height; i++)
        {
            for (int j = 0; j < grid.Size.Width; j++)
            {
                Assert.IsNotNull(grid.GetGridObject(i,j));
            }
        }
    }
    
    [Test]
    public void GetWorldPositionTests()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));
    
        Assert.AreEqual(Vector3.Zero, grid.GetWorldPosition(0, 0));
        Assert.AreEqual(new Vector3(0,10,0), grid.GetWorldPosition(0, 1));
        Assert.AreEqual(new Vector3(0,20,0), grid.GetWorldPosition(0, 2));
        Assert.AreEqual(new Vector3(10,0,0), grid.GetWorldPosition(1, 0));
        Assert.AreEqual(new Vector3(20,0,0), grid.GetWorldPosition(2, 0));
        Assert.AreEqual(new Vector3(10,10,0), grid.GetWorldPosition(1, 1));
        Assert.AreEqual(new Vector3(20,20,0), grid.GetWorldPosition(2, 2));
        
        Assert.AreEqual(Vector3.Zero, grid.GetWorldPosition(new Location(0,0)));
        Assert.AreEqual(new Vector3(0,10,0), grid.GetWorldPosition(new Location(0,1)));
        Assert.AreEqual(new Vector3(0,20,0), grid.GetWorldPosition(new Location(0, 2)));
        Assert.AreEqual(new Vector3(10,0,0), grid.GetWorldPosition(new Location(1, 0)));
        Assert.AreEqual(new Vector3(20,0,0), grid.GetWorldPosition(new Location(2, 0)));
        Assert.AreEqual(new Vector3(10,10,0), grid.GetWorldPosition(new Location(1, 1)));
        Assert.AreEqual(new Vector3(20,20,0), grid.GetWorldPosition(new Location(2, 2)));

    }

    
    

    [Test]
    public void SettingGridObjectsValue()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));

        IntGridObject g1 = new IntGridObject(grid, new Location(0, 0));
        g1.Value = 10;
        
        Assert.AreEqual(0, grid.GetGridObject(0,0).Value);
        grid.SetGridObject(g1.Location, g1);
        Assert.AreEqual(10, grid.GetGridObject(0,0).Value);
        
        IntGridObject g2 = new IntGridObject(grid, new Location(1, 1));
        g2.Value = 20;
        
        Assert.AreEqual(0, grid.GetGridObject(1,1).Value);
        grid.SetGridObject(new Vector3(15,15,0), g2);
        Assert.AreEqual(20, grid.GetGridObject(1,1).Value);
        
        Assert.AreEqual(0, grid.GetGridObject(2,2).Value);
        grid.SetGridObject(2,2, g2);
        Assert.AreEqual(20, grid.GetGridObject(2,2).Value);
        
    }

    [Test]
    public void InvokingOnGridObjectChangedWorks()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));

        bool invoked = false;
        grid.OnGridObjectChanged += (sender, args) => invoked = true;
        grid.InvokeOnGridObjectChanged(0,0);
        Assert.IsTrue(invoked);
        
        invoked = false;
        grid.InvokeOnGridObjectChanged(new Location(0,0));
        Assert.IsTrue(invoked);
    }


    [Test]
    public void GetGridObjectsTests()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));

        IntGridObject g1 = new IntGridObject(grid, new Location(0, 0));
        g1.Value = 10;
        
        Assert.AreEqual(0, grid.GetGridObject(0,0).Value);
        Assert.AreEqual(0, grid.GetGridObject(new Vector3(5,5,0)).Value);
        grid.SetGridObject(g1.Location, g1);
        Assert.AreEqual(10, grid.GetGridObject(0,0).Value);
        Assert.AreEqual(10, grid.GetGridObject(new Vector3(5,5,0)).Value);
        
        
    }


    [Test]
    public void GetXYWorks()
    {
        Grid<IntGridObject> grid = new Grid<IntGridObject>(new Size(3, 3), 10, Vector3.Zero,
            (Grid<IntGridObject> g, Location l) => new IntGridObject(g,l));

        int x, y;
        grid.GetXY(new Vector3(5, 5, 0), out x, out y);
        Assert.AreEqual(0, x);
        Assert.AreEqual(0, y);
        
        grid.GetXY(new Vector3(25, 25, 0), out x, out y);
        Assert.AreEqual(2, x);
        Assert.AreEqual(2, y);
        
        grid.GetXY(new Vector3(15, 25, 0), out x, out y);
        Assert.AreEqual(1, x);
        Assert.AreEqual(2, y);
        
        
    }

    
}
