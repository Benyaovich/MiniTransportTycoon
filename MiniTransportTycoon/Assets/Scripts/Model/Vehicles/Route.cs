using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;
using Model.Cells;
using PlasticGui;

public class Route
{
    PathHandler _pathHandler;
    public Queue<Location> Vertices { get; private set; }
    public Location PreviousVertex { get; private set; }
    public Location CurrentVertex { get; private set; }
    public Location NextVertex { get; private set; }

    public Location PreviousPosition { get; private set; }
    public Location CurrentPosition {get; private set; }
    public Location NextPosition { get; private set; }
    public bool IsTurning { get; private set; }
    private bool currentlyStuck = false;
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
    
    public Route(List<Location> vertices, PathHandler pathHandler)
    {
        _pathHandler = pathHandler;
        
        SetUp(vertices);
    }

    public void SetUp(List<Location> vertices)
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
        if (currentlyStuck)
        {
            try
            {
                RecalculateRoute();
            }
            catch (Exception e)
            {
                return;
            }
        }
        
        PreviousPosition = CurrentPosition;
        CurrentPosition += CurrentDirection.ToLocation();
        
        if (CurrentPosition == NextVertex)
        {
            try
            {
                StepVertex();
            }
            catch (Exception e)
            {
                return;
            }
        }
        NextPosition = CurrentPosition + CurrentDirection;
        SetIsTurning();
    }

    
    
    public void StepVertex()
    {
        if (!_pathHandler.Graph.ContainsVertex(NextVertex))
        {
            RecalculateRoute();
        }
        else
        {
            Vertices.Enqueue(CurrentVertex);
            PreviousVertex = CurrentVertex;
            CurrentVertex = NextVertex;
            NextVertex = Vertices.Dequeue();
        }
    }

    private void RecalculateRoute()
    {
        try
        {
            List<Location> newLocs = _pathHandler.GetPathFromRoute(_pathHandler.Graph.Vertices);
            
            SetUp(newLocs);
        }
        catch (Exception e)
        {
            currentlyStuck = true;
            throw new Exception("Nincs elerheto ut");
        }
        
        currentlyStuck = false;
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
