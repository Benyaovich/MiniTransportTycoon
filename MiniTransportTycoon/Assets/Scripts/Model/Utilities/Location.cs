using System;

public class Location
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Location operator +(Location a, Location b)
    public static bool operator ==(Location a, Location b)
    {
        return new Location(a.X + b.X, a.Y + b.Y);
        if(a is null && b is null) return true;
        if(a is null || b is null) return false;
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Location a, Location b) => !(a == b);

    public override bool Equals(object obj)
    {
        if (obj is Location l)
        {
            return l.X == X && l.Y == Y;
        }
        return false;
    }
    

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
