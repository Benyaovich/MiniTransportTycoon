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

    public Location PreviousPosition { get; private set; }
    public Location CurrentPosition {get; private set; }
    public Location NextPosition { get; private set; }
    public bool IsTurning { get; private set; }
    public Direction TurningDirection => (NextPosition - CurrentPosition).ToDirection();
    
    public Direction CurrentDirection => (NextVertex - CurrentVertex).ToDirection();

    public Direction NextDirection
    {
        get
        {
            if (Vertices.Count == 0) return PreviousDirection;
            return (Vertices.Peek() - NextVertex).ToDirection();
        }
    }

    public Direction PreviousDirection => (CurrentVertex - PreviousVertex).ToDirection();
    
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

        PreviousPosition = CurrentVertex;
        CurrentPosition = CurrentVertex;
        NextPosition = CurrentPosition + CurrentDirection;
    }

    public void Step()
    {
        PreviousPosition = CurrentPosition;
        CurrentPosition += CurrentDirection.ToLocation();
        
        if (CurrentPosition == NextVertex)
        {
            StepVertex();
        }
        NextPosition = CurrentPosition + CurrentDirection;
        SetIsTurning();
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
    
    private void SetIsTurning()
    {
        IsTurning = (CurrentPosition - PreviousPosition).ToDirection() !=
                    (NextPosition - CurrentPosition).ToDirection();
    }
}
