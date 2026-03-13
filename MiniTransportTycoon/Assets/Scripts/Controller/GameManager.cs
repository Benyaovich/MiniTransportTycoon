using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IBuildingManager _buildingManager;
    private IGraph _graph;
    private PathHandler _pathHandler;
    private IGraphBuilder _graphBuilder;
    private HighlightService _highlightService;

    private void Start()
    {
        _buildingManager = GridManager.Instance!.BuildingManager;
        _graph = new Graph();
        _pathHandler = new PathHandler(_graph);
        _graphBuilder = new GraphBuilder<GridObject>(GridManager.Instance.Grid, _graph);
        _highlightService = GridManager.Instance!.HighlightService;
        GridManager.Instance.SetPathHandler(_pathHandler);
        
        _buildingManager.OnRoadCellBuilt += BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished += BuildingManagerOnRoadCellDemolished;
    }
    
    private void OnDisable()
    {
        _buildingManager.OnRoadCellBuilt -= BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished -= BuildingManagerOnRoadCellDemolished;
    }

    private void BuildingManagerOnRoadCellBuilt(object sender, Location location)
    {
        _graphBuilder.CreateConnectionsAt(location);
    }
    
    private void BuildingManagerOnRoadCellDemolished(object sender, Location location)
    {
        _graphBuilder.RemoveConnectionsAt(location);
    }
}
