using System;

[Serializable]
public class SerializableLocation
{
    public int x;
    public int y;

    public SerializableLocation(Location location)
    {
        x = location.X;
        y = location.Y;
    }

    public SerializableLocation()
    {
        
    }
}
