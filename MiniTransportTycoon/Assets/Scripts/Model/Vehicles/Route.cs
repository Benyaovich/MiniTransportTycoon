using System;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;
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
    public bool CurrentlyStuck { get;private set; }
    public bool Turns180Happened { get; private set; }
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

    public Route(
        Queue<Location> vertices,
        PathHandler pathHandler,
        Location previousVertex,
        Location currentVertex,
        Location nextVertex,
        Location previousPosition,
        Location currentPosition,
        Location nextPosition,
        bool isTurning,
        bool currentlyStuck,
        bool turns180Happened,
        bool turns180Finished
    )
    {
        Vertices = vertices;
        _pathHandler = pathHandler;
        PreviousVertex = previousVertex;
        CurrentVertex = currentVertex;
        NextVertex = nextVertex;
        PreviousPosition = previousPosition;
        CurrentPosition = currentPosition;
        NextPosition = nextPosition;
        IsTurning = isTurning;
        CurrentlyStuck = currentlyStuck;
        Turns180Happened = turns180Happened;
        Turns180Finished = turns180Finished;
    }

    public void SetUp(List<Location> vertices)
    {
        List<Location> normalizedVertices = new();
        foreach (Location vertex in vertices)
        {
            if (normalizedVertices.Count > 0 && normalizedVertices[^1] == vertex) continue;
            normalizedVertices.Add(vertex);
        }

        //ha a palyanak elso es utolso eleme egyezik
        if (normalizedVertices[0] == normalizedVertices[^1])
        {
            normalizedVertices.RemoveAt(normalizedVertices.Count - 1);
        }

        if (normalizedVertices.Count < 2)
        {
            throw new ArgumentException("Route setup requires at least two different vertices.");
        }
        
        Vertices = new Queue<Location>();
        foreach (var vertex in normalizedVertices)
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
        if (CurrentlyStuck)
        {
            StepVertex();
            if (CurrentlyStuck) return;
        }
        
        if (Turns180Happened)
        {
            StepVertex();
            if (CurrentlyStuck) return;
            Turns180Happened = false;
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
                Turns180Happened = true;
                IsTurning = true;
                NextPosition = CurrentPosition;
                return;
            }
            
            StepVertex();
            if (CurrentlyStuck) return;
        }
        NextPosition = CurrentPosition + CurrentDirection;
        Turns180Finished = false;
        SetIsTurning();
    }

    
    
    public void StepVertex()
    {
        if (!_pathHandler.Graph.ContainsVertex(NextVertex) || !_pathHandler.Graph.Edges.Contains(new Edge(CurrentVertex, NextVertex)))
        {
            if (!TryRecalculateRouteFromCurrentState())
            {
                CurrentlyStuck = true;
                return;
            }

            // A successful recalculation already positions the route at the
            // reached vertex and assigns the next target vertex.
            return;
        }
        
        CurrentlyStuck = false;
        
        Vertices.Enqueue(CurrentVertex);
        PreviousVertex = CurrentVertex;
        CurrentVertex = NextVertex; 
        NextVertex = Vertices.Dequeue();
        
    }

    private bool TryRecalculateRouteFromCurrentState()
    {
        try
        {
            Location previousPositionBeforeRepath = PreviousPosition;
            Location currentPositionBeforeRepath = CurrentPosition;

            List<Location> newLocs = _pathHandler.GetPathFromRoute(BuildRouteCycleForRepath());
            SetUp(newLocs);

            PreviousPosition = previousPositionBeforeRepath;
            CurrentPosition = currentPositionBeforeRepath;
            NextPosition = CurrentPosition + CurrentDirection;
            SetIsTurning();

            CurrentlyStuck = false;
            return true;
        }
        catch (Exception)
        {
            CurrentlyStuck = true;
            return false;
        }
    }

    private List<Location> BuildRouteCycleForRepath()
    {
        List<Location> routeCycle = new() { NextVertex };
        routeCycle.AddRange(Vertices);
        routeCycle.Add(CurrentVertex);
        routeCycle.Add(NextVertex);
        return routeCycle;
    }

    public bool ContainsVertex(Location location)
    {
        return (Vertices.Contains(location) || location == CurrentVertex ||  location == NextVertex);
    }
    
    private void SetIsTurning()
    {
        if (CurrentPosition == PreviousPosition || NextPosition == CurrentPosition)
        {
            IsTurning = false;
            return;
        }

        Direction currDir = (CurrentPosition - PreviousPosition).ToDirection();
        Direction nextDir = (NextPosition - CurrentPosition).ToDirection();
        IsTurning = currDir != nextDir;
    }
}
