using JetBrains.Annotations;

public class SearchNode
{
    [CanBeNull] public Location Parent { get; set; }
    public int Distance { get; set; }
    public Location Vertex { get; }

    public SearchNode(int distance, Location vertex,Location parent = null)
    {
        Distance = distance;
        Vertex = vertex;
        Parent = parent;
    }
}