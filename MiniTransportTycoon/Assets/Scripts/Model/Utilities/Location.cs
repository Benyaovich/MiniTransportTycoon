using System;
using Model.Enumerations;

public class Location
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    
    
    public static Location operator +(Location a, Location b) => new Location(a.X + b.X, a.Y + b.Y);

    public static Location operator +(Location a, Direction dir)
    {
        Location directionAsLocation = dir.ToLocation();
        return new Location(a.X + directionAsLocation.X, a.Y + directionAsLocation.Y);
    }
    
    public static Location operator -(Location a, Location b) => new Location(a.X - b.X, a.Y - b.Y);
    public static Location operator -(Location a, Direction dir)
    {
        Location directionAsLocation = dir.ToLocation();
        return new Location(a.X - directionAsLocation.X, a.Y - directionAsLocation.Y);
    }

    public static bool operator ==(Location a, Location b)
    {
        if(a is null && b is null) return true;
        if(a is null || b is null) return false;
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Location a, Location b) => !(a == b);

    public override bool Equals(object obj)
    {
        if (obj is Location location) return this == location;
        return false;
    }
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
}
