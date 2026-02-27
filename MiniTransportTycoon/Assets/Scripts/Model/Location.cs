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
    {
        return new Location(a.X + b.X, a.Y + b.Y);
    }

    public static Location operator -(Location a, Location b)
    {
        return new Location(a.X - b.X, a.Y - b.Y);
    }
}
