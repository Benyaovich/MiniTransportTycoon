using System;
using Model.Enumerations;

public static class Converter
{
    public static Direction ToDirection(this Location location, Location nextLocation)
    {
        int deltaX = nextLocation.X - location.X;
        int deltaY = nextLocation.Y - location.Y;
        
        return (deltaX, deltaY) switch
        {
            (0, -1) => Direction.Up,
            (0, 1) => Direction.Down,
            (-1, 0) => Direction.Left,
            (1, 0) => Direction.Right,
            (0,0) => throw new ArgumentException("The two Locations are the same"),
            _ => throw new Exception($"Can not convert this movement ({location.X},{location.Y} -> {nextLocation.X},{nextLocation.Y}) to direction.")
        };
    }

    public static Location ToLocation(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Location(0, -1),
            Direction.Down => new Location(0, 1),
            Direction.Left => new Location(-1, 0),
            Direction.Right => new Location(1, 0),
            _ => throw new Exception($"Can not convert this direction ({direction.ToString()}) to location.")
        };
    }

    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new Exception($"Opposite of {direction.ToString()} is not defined yet.")
        };
    }
}
