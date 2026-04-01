using System;
using Model.Cells.Grid;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    private CellBuildingManager _cellBuildingManager;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private IGraph _graph;
    private PathHandler _pathHandler;
    private IGraphBuilder _graphBuilder;
    private HighlightService _highlightService;
    private IBuildSelectionManager _buildSelectionManager;

    private GameplayState _gameplayState;

    private void Awake()
    {
        _cellBuildingManager = GridManager.Instance!.CellBuildingManager;
        _dynamicRoadBuildingManager = GridManager.Instance!.DynamicRoadBuildingManager;
        _buildSelectionManager = new BuildSelectionManager();
        _graph = new Graph();
        _pathHandler = new PathHandler(_graph);
        _graphBuilder = new GraphBuilder(GridManager.Instance.Grid, _graph);
        GridManager.Instance.SetBuildSelectionManager(_buildSelectionManager);
        RouteCreationManager.Instance.Setup(_pathHandler);
        
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
                GridManager.Instance!.HandleBuildSelectionInput();
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
        _buildSelectionManager.ClearSelectedObjectType();
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
