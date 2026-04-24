using System;
using System.Collections.Generic;

[Serializable]
public class SRoute
{
    public Queue<SLocation> vertices = new Queue<SLocation>();
    public SLocation previousVertex;
    public SLocation currentVertex;
    public SLocation nextVertex;
    public SLocation previousPosition;
    public SLocation currentPosition;
    public SLocation nextPosition;
    public bool isTurning;
    public bool currentlyStuck;
    public bool turns180Happened;
    public bool turns180Finished;

    public SRoute(Route route)
    {
        foreach (var item in route.Vertices)
            vertices.Enqueue(new SLocation(item));
        previousVertex = new SLocation(route.PreviousVertex);
        currentVertex = new SLocation(route.CurrentVertex);
        nextVertex = new SLocation(route.NextVertex);
        previousPosition = new SLocation(route.PreviousPosition);
        currentPosition = new SLocation(route.CurrentPosition);
        nextPosition = new SLocation(route.NextPosition);
        isTurning = route.IsTurning;
        currentlyStuck = route.CurrentlyStuck;
        turns180Happened = route.Turns180Happened;
        turns180Finished = route.Turns180Finished;
    }
    
    public SRoute() { }
}