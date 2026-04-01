public interface IGraphBuilder
{
    public void CreateConnectionsAt(RoadCell roadCell);
    public void RemoveConnectionsAt(RoadCell roadCell);

    public void RefreshConnectionsAt(RoadCell roadCell);
}
