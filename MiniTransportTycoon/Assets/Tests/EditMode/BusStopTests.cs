using System.Numerics;
using NUnit.Framework;

public class BusStopTests
{
    private Grid<MockGridObject> _grid;
    [SetUp]
    public void Init()
    {
        _grid= new Grid<MockGridObject>(new Size(5, 5), 10, Vector3.Zero,
            (g, l) => new MockGridObject(g,l));
    }
    
    [Test]
    public void BusStopTestsSimplePasses()
    {
        // implementálni kell a városok gird logikáját hogy lehessen tesztelni
    }
    
    
    private class MockGridObject : IHasCellModel
    {
        public Cell Model { get; set; }
        public void SetModel(Cell cell)
        {
            Model = cell;
        }

        public void ClearModel()
        {
            Model = null;
        }

        public MockGridObject(Grid<MockGridObject> g, Location l)
        {
        }
    }
}
