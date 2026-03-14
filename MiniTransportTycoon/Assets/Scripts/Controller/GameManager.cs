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
        _graphBuilder = new GraphBuilder<GridObject>(GridManager.Instance.Grid, _graph);
        _highlightService = new HighlightService(GridManager.Instance!.Grid);
        GridManager.Instance.SetBuildSelectionManager(_buildSelectionManager);
        RouteCreationManager.Instance.Setup(_highlightService, _pathHandler);
        
        _gameplayState = GameplayState.Building;
    }

    private void OnEnable()
    {
        _buildingManager.OnRoadCellBuilt += BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished += BuildingManagerOnRoadCellDemolished;
    }

    private void OnDisable()
    {
        _buildingManager.OnRoadCellBuilt -= BuildingManagerOnRoadCellBuilt;
        _buildingManager.OnRoadCellDemolished -= BuildingManagerOnRoadCellDemolished;
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (_gameplayState == GameplayState.Building)
            {
                _gameplayState = GameplayState.RouteCreating;
                _buildSelectionManager.ClearSelectedObjectType();
                RouteCreationManager.Instance!.StartRouteCreation();
            }
            else if (_gameplayState == GameplayState.RouteCreating)
            {
                _gameplayState = GameplayState.Building;
                RouteCreationManager.Instance!.ExitRouteCreation();
            }
        }
        
        switch (_gameplayState)
        {
            case GameplayState.Building:
                GridManager.Instance!.HandleBuildSelectionInput();
                break;
            
            case GameplayState.RouteCreating:
                

                break;
        }
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
