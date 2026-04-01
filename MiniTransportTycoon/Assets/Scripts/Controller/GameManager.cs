using System;
using Model.Cells.Grid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private IGraphBuilder _graphBuilder;
    private HighlightService _highlightService;

    private GameplayState _gameplayState;

    private void Awake()
    {
        _dynamicRoadBuildingManager = GridManager.Instance!.DynamicRoadBuildingManager;
        Graph graph = new Graph();
        PathHandler pathHandler = new PathHandler(graph);
        _graphBuilder = new GraphBuilder(GridManager.Instance.Grid, graph);
        RouteCreationManager.Instance.Setup(pathHandler);
        
        _gameplayState = GameplayState.Building;
    }

    private void OnEnable()
    {
        _dynamicRoadBuildingManager.OnRoadCellBuilt += DynamicRoadBuildingManagerOnRoadCellBuilt;
        _dynamicRoadBuildingManager.OnRoadCellDemolished += DynamicRoadBuildingManagerOnRoadCellDemolished;
        _dynamicRoadBuildingManager.OnRoadCellRefreshed += DynamicRoadBuildingManagerOnRoadCellRefreshed;
        RouteCreationManager.Instance.OnRouteCreationStarted += InstanceOnRouteCreationStarted;
        RouteCreationManager.Instance.OnRouteCreationFinished += InstanceOnRouteCreationFinished;
    }
    

    private void OnDisable()
    {
        _dynamicRoadBuildingManager.OnRoadCellBuilt -= DynamicRoadBuildingManagerOnRoadCellBuilt;
        _dynamicRoadBuildingManager.OnRoadCellDemolished -= DynamicRoadBuildingManagerOnRoadCellDemolished;
        _dynamicRoadBuildingManager.OnRoadCellRefreshed -= DynamicRoadBuildingManagerOnRoadCellRefreshed;
        RouteCreationManager.Instance.OnRouteCreationStarted -= InstanceOnRouteCreationStarted;
        RouteCreationManager.Instance.OnRouteCreationFinished -= InstanceOnRouteCreationFinished;
    }

    private void Update()
    {
        
        switch (_gameplayState)
        {
            case GameplayState.Building:
                BuildSelectionManager.Instance!.HandleBuildSelectionInput();
                break;
            
            case GameplayState.RouteCreating:
                

                break;
        }
    }

    
    private void InstanceOnRouteCreationFinished(object sender, EventArgs e)
    {
        _gameplayState = GameplayState.Building;
    }

    private void InstanceOnRouteCreationStarted(object sender, EventArgs e)
    {
        BuildSelectionManager.Instance.ClearSelectedObjectType();
        _gameplayState = GameplayState.RouteCreating;
    }
    
    private void DynamicRoadBuildingManagerOnRoadCellBuilt(object sender, RoadCell roadCell)
    {
        _graphBuilder.CreateConnectionsAt(roadCell);
    }
    
    private void DynamicRoadBuildingManagerOnRoadCellDemolished(object sender, RoadCell roadCell)
    {
        _graphBuilder.RemoveConnectionsAt(roadCell);
    }
    
    private void DynamicRoadBuildingManagerOnRoadCellRefreshed(object sender, RoadCell roadCell)
    {
        _graphBuilder.RefreshConnectionsAt(roadCell);
    }
    
}
public enum GameplayState
{
    Building, RouteCreating
}
