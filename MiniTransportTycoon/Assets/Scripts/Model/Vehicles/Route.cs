using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;
using Model.Cells;
using NUnit.Framework;

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
    public bool Turns180happened { get; private set; }
    public bool Turns180Finished { get; private set; }
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
            catch (Exception ex) when (ex is InvalidOperationException)
            {
                return;
            }
        }
        
        if (Turns180happened)
        {
            StepVertex();
            Turns180happened = false;
            Turns180Finished = true;
            PreviousPosition = CurrentPosition;
            NextPosition = CurrentPosition + CurrentDirection;
            IsTurning = true;
            return;
        } 
        
        
        PreviousPosition = CurrentPosition;
        CurrentPosition += CurrentDirection.ToLocation();
        
        if (CurrentPosition == NextVertex)
        {
            if (CurrentDirection.Opposite() == NextDirection)
            {
                Turns180happened = true;
                IsTurning = true;
                NextPosition = CurrentPosition;
                return;
            }
            else
            {
                StepVertex();
            }
        }
        NextPosition = CurrentPosition + CurrentDirection;
        Turns180Finished = false;
        SetIsTurning();
    }

    
    
    public void StepVertex()
    {
        if (!_pathHandler.Graph.ContainsVertex(NextVertex))
        {
            try
            {
                RecalculateRoute();
            }
            catch (InvalidOperationException) { }
            return;
        }
        
        Vertices.Enqueue(CurrentVertex);
        PreviousVertex = CurrentVertex;
        CurrentVertex = NextVertex; 
        NextVertex = Vertices.Dequeue();
        
    }

    private void RecalculateRoute()
    {
        try
        {
            List<Location> newLocs = _pathHandler.GetPathFromRoute(_pathHandler.Graph.Vertices);

            SetUp(newLocs);
        }
        catch (ArgumentException ex)
        {
            currentlyStuck = true;
            throw new InvalidOperationException("Nem lehet utat csinalni a letezo csucsokbol. Ennek oka: " + ex.Message);
        }
        
        currentlyStuck = false;
    }

    public bool ContainsVertex(Location location)
    {
        return (Vertices.Contains(location) || location == CurrentVertex ||  location == NextVertex);
    }
    
    private void SetIsTurning()
    {
        Direction currDir = (CurrentPosition - PreviousPosition).ToDirection();
        Direction nextDir = (NextPosition - CurrentPosition).ToDirection();
        IsTurning = currDir != nextDir;
    }
}
