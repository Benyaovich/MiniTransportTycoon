using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.Grid;
using Model.RoadSystem;
using UnityEngine;

public class RouteCreationManager : MonoBehaviour
{
    public static RouteCreationManager Instance { get; private set; }
    public event EventHandler<List<Location>> OnRouteCreated;
    public event EventHandler OnRouteCreationStarted; 
    public event EventHandler OnRouteCreationFinished; 
    public bool InRouteCreation { get; private set; }
    public PathHandler PathHandler => _pathHandler;
    
    private PathHandler _pathHandler;
    private Grid<ModelGridObject> _grid;
    private List<Location> _selectedVertices = new();
    private List<Location> _availableVertices = new();


    private void Awake()
    {
        Instance = this;
        
        _grid = GridManager.Instance!.Grid;
    }
    
    private void OnEnable()
    {
        GameInput.Instance.OnLeftClickPressed += GameInputOnLeftClickPressed;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnLeftClickPressed -= GameInputOnLeftClickPressed;
    }
    
    public void Setup(PathHandler pathHandler)
    {
        _pathHandler = pathHandler;
    }

    public void StartRouteCreation()
    {
        _selectedVertices.Clear();
        _availableVertices.Clear();
        HighlightManager.Instance.HighlightService.EnableHighlight(_pathHandler.Graph.Vertices);
        InRouteCreation = true;
        OnRouteCreationStarted?.Invoke(this, EventArgs.Empty);
    }

    public void ExitRouteCreation()
    {
        HighlightManager.Instance.HighlightService.DisableHighlight(_pathHandler.Graph.Vertices);
        InRouteCreation = false;
        OnRouteCreationFinished?.Invoke(this, EventArgs.Empty);
    }
    
    private void GameInputOnLeftClickPressed(object sender, EventArgs e)
    {
        if (!InRouteCreation) return;
        
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        if (_grid.GetGridObject(x, y)?.Model is not RoadCell roadCell) return;
        if (!roadCell.IsVertexPoint) return;
        
        TryAddVertexLocation(new Location(x, y));
        if (_selectedVertices.Count == 0) return;
        
        bool finished = IsRouteCreationProcessFinished();
        
        if(!finished){ RefreshHighlights(); }
        else
        {
            HighlightManager.Instance.HighlightService.DisableHighlight(_availableVertices);
            OnRouteCreated?.Invoke(this, _pathHandler.GetPathFromRoute(_selectedVertices));
            
            ExitRouteCreation();
        }
    }

    private void TryAddVertexLocation(Location location)
    {
        if (_selectedVertices.Count == 0)
        {
            _availableVertices = GraphAlgorithms.GetReachableGraphFromBfsTable(_pathHandler.Graph,
                GraphAlgorithms.Bfs(location, _pathHandler.Graph)).Vertices;
            if (_availableVertices.Count == 1) return;
            _selectedVertices.Add(location);
        }
        else
        {
            if (_selectedVertices[^1] == location) return;
            if (!_availableVertices.Contains(location)) return;
            _selectedVertices.Add(location);
        }
        
    }

    private bool IsRouteCreationProcessFinished()
    {
        if (_selectedVertices.Count < 3) return false;
        if (_selectedVertices[0] != _selectedVertices[^1]) return false;
        return true;
    }
        

    private List<Location> GetHighlightableVertices()
    {
        return _availableVertices.Where(x => x != _selectedVertices[^1]).ToList();
    }

    private void RefreshHighlights()
    {
        HighlightManager.Instance.HighlightService.DisableHighlight(_pathHandler.Graph.Vertices);
        HighlightManager.Instance.HighlightService.EnableHighlight(GetHighlightableVertices());
    }


    
}
