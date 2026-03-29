using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    private IBuildingManager _buildingManager;
    private IGraph _graph;
    private PathHandler _pathHandler;
    private IGraphBuilder _graphBuilder;
    private HighlightService _highlightService;
    private IBuildSelectionManager _buildSelectionManager;

    private GameplayState _gameplayState;

    private void Awake()
    {
        _buildingManager = GridManager.Instance!.BuildingManager;
        _buildSelectionManager = new BuildSelectionManager();
        _graph = new Graph();
        _pathHandler = new PathHandler(_graph);
        _graphBuilder = new GraphBuilder(GridManager.Instance.Grid, _graph);
        _highlightService = new HighlightService(GridManager.Instance!.Grid);
        GridManager.Instance.SetBuildSelectionManager(_buildSelectionManager);
        RouteCreationManager.Instance.Setup(_highlightService, _pathHandler);
        
        _gameplayState = GameplayState.Building;
    }

    private void OnEnable()
    {
        _buildingManager.OnRoadCellBuilt += BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished += BuildingManagerOnRoadCellDemolished;
        RouteCreationManager.Instance.OnRouteCreationStarted += InstanceOnRouteCreationStarted;
        RouteCreationManager.Instance.OnRouteCreationFinished += InstanceOnRouteCreationFinished;
    }

    

    private void OnDisable()
    {
        _buildingManager.OnRoadCellBuilt -= BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished -= BuildingManagerOnRoadCellDemolished;
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
    
    private void BuildingManagerOnRoadCellBuilt(object sender, Location location)
    {
        _graphBuilder.CreateConnectionsAt(location);
    }
    
    private void BuildingManagerOnRoadCellDemolished(object sender, Location location)
    {
        _graphBuilder.RemoveConnectionsAt(location);
    }
    
}
public enum GameplayState
{
    Building, RouteCreating
}
