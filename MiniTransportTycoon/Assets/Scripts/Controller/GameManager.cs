using System;
using Controller.Building;
using Controller.Grid;
using Model.Cells.Grid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float DeltaTime => Time.deltaTime * _gameSpeedMultiplier;
    public float GameSpeedMultiplier => _gameSpeedMultiplier;
    
    private float _gameSpeedMultiplier = 1;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private IGraphBuilder _graphBuilder;
    private HighlightService _highlightService;

    private GameplayState _gameplayState;

    
    private void Awake()
    {
        Instance = this;
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
        BuildSelectionManager.Instance.OnDynamicRoadSelected += InstanceOnDynamicRoadSelected; 
        BuildSelectionManager.Instance.OnBuildingSelected += InstanceOnBuildingSelected; 
    }

    


    private void OnDisable()
    {
        _dynamicRoadBuildingManager.OnRoadCellBuilt -= DynamicRoadBuildingManagerOnRoadCellBuilt;
        _dynamicRoadBuildingManager.OnRoadCellDemolished -= DynamicRoadBuildingManagerOnRoadCellDemolished;
        _dynamicRoadBuildingManager.OnRoadCellRefreshed -= DynamicRoadBuildingManagerOnRoadCellRefreshed;
        RouteCreationManager.Instance.OnRouteCreationStarted -= InstanceOnRouteCreationStarted;
        RouteCreationManager.Instance.OnRouteCreationFinished -= InstanceOnRouteCreationFinished;
        BuildSelectionManager.Instance.OnDynamicRoadSelected -= InstanceOnDynamicRoadSelected; 
        BuildSelectionManager.Instance.OnBuildingSelected -= InstanceOnBuildingSelected; 
    }

    private void Update()
    {
        if (_gameplayState == GameplayState.Building ||
            _gameplayState == GameplayState.DynamicRoadBuilding)
        {
            BuildSelectionManager.Instance!.HandleBuildSelectionInput();
        }
    }

    
    private void InstanceOnRouteCreationFinished(object sender, EventArgs e)
    {
        _gameplayState = GameplayState.Building;
    }

    private void InstanceOnRouteCreationStarted(object sender, EventArgs e)
    {
        BuildSelectionManager.Instance.ClearSelectedObjectType();
        DynamicRoadBuilder.Instance.SetActive(false);
        _gameplayState = GameplayState.RouteCreating;
    }
    
    private void InstanceOnDynamicRoadSelected(object sender, EventArgs e)
    {
        _gameplayState = GameplayState.DynamicRoadBuilding;
        DynamicRoadBuilder.Instance.ResetRoadBuilder();
        DynamicRoadBuilder.Instance.SetActive(true);
    }
    
    private void InstanceOnBuildingSelected(object sender, EventArgs e)
    {
        _gameplayState = GameplayState.Building;
        DynamicRoadBuilder.Instance.SetActive(false);
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

    public void SetGameSpeedMultiplier(float value) => _gameSpeedMultiplier = value;

}
public enum GameplayState
{
    Building, RouteCreating, DynamicRoadBuilding
}
