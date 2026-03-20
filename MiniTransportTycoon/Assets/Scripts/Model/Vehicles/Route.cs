using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;

public class Route
{
    public Queue<Location> Vertices { get; private set; }
    public Location PreviousVertex { get; private set; }
    public Location CurrentVertex { get; private set; }
    public Location NextVertex { get; private set; }
    
    public Location CurrentPosition {get; private set; }
    
    public Direction CurrentDirection => CurrentVertex.ToDirection(NextVertex);

    public Direction NextDirection => NextVertex.ToDirection(Vertices.Peek());

    public Direction PreviousDirection => PreviousVertex.ToDirection(CurrentVertex);
    
    private int index;
    
    public Route(List<Location> vertices)
    {
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
        
        PreviousVertex = Vertices.Last();
        CurrentVertex = Vertices.Dequeue();
        NextVertex = Vertices.Dequeue();
        
        CurrentPosition = CurrentVertex;
    }

    public void Step()
    {
        if (CurrentPosition == NextVertex)
        {
            StepVertex();
        }
        
        CurrentVertex += CurrentDirection;
    }
    
    public void StepVertex()
    {
        Vertices.Enqueue(CurrentVertex);
        PreviousVertex = CurrentVertex;
        CurrentVertex = NextVertex;
        NextVertex = Vertices.Dequeue();
    }

    public bool ContainsVertex(Location location)
    {
        return (Vertices.Contains(location) || location == CurrentVertex ||  location == NextVertex);
    }
}
