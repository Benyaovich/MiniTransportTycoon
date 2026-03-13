using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;

public class Route
{
    public Queue<Location> Vertices { get; private set; }
    public Location PreviousLocation { get; private set; }
    public Location CurrentLocation { get; private set; }
    public Location NextLocation { get; private set; }

    public Direction CurrentDirection
    {
        get
        {
            return CurrentLocation.ToDirection(NextLocation);
        }
        private set {}
    }
    
    private int index;
    
    public Route(List<Location> vertices)
    {
        if (vertices.Count < 2)
        {
            throw new ArgumentException("Ebbol nem lehet korpalyat csinalni, kevesebb mint 2 allomas");
        }
        //ha a palyanak elso es utolso eleme egyezik
        if (vertices[0] == vertices[^1])
        {
            vertices.RemoveAt(vertices.Count - 1);
        }
        
        Vertices = new Queue<Location>();
        foreach (var vertex in vertices)
        {
            Vertices.Enqueue(vertex);
        }

        PreviousLocation = Vertices.Last();
        CurrentLocation = Vertices.Dequeue();
        NextLocation = Vertices.Dequeue();
    }

    public void StepVertex()
    {
        Vertices.Enqueue(CurrentLocation);
        PreviousLocation = CurrentLocation;
        CurrentLocation = NextLocation;
        NextLocation = Vertices.Dequeue();
    }

    public bool ContainsVertex(Location location)
    {
        return Vertices.Contains(location);
    }
    
    
}
