#nullable enable
using System;

public class OnModelChangedEventArgs : EventArgs
{
    public Cell? Cell;
    public Location Location;

    public OnModelChangedEventArgs(Cell? cell, Location location)
    {
        Cell = cell;
        Location = location;
    }
}