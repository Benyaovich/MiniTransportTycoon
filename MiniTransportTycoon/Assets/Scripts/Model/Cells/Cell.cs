
using System.Collections.Generic;

public class Cell
{
    public Location Origin { get; private set; }
    public Size Size { get; protected set; }
    public bool Destroyable { get; private set; }
    public int RotationDegrees { get; private set; }
    
    public Cell(Location origin, Size size = null, bool destroyable = true)
    {
        Origin = origin;
        Size = size ?? new Size(1, 1);
        Destroyable = destroyable;
        RotationDegrees = 0;
    }

    public void SetRotation(int rotationDegrees)
    {
        RotationDegrees = rotationDegrees;
    }
    
    public List<Location> GetGridPositionList()
    {
        List<Location> locations = new();
        
        for (int y = 0; y < Size.Height; y++)
        {
            for (int x = 0; x < Size.Width; x++)
            {
                locations.Add(Origin + new Location(x,y));
            }
        }

        return locations;
    }
}
