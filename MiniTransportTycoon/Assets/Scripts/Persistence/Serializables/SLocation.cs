using System;

[Serializable]
public class SLocation
{
    public int x;
    public int y;

    public SLocation(Location location)
    {
        x = location.X;
        y = location.Y;
    }

    public SLocation()
    {
        
    }
}
