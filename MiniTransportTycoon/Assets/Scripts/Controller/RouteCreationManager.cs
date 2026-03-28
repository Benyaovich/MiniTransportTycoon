using System;
using System.Collections.Generic;
using System.Linq;
using Model.RoadSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class RouteCreationManager : MonoBehaviour
{
    public static RouteCreationManager Instance { get; private set; }
    public event EventHandler<List<Location>> OnRouteCreated;
    private HighlightService _highlightService;
    private PathHandler _pathHandler;
    private Grid<ModelGridObject> _grid;
    private List<Location> _selectedVertices = new();
    private List<Location> _availableVertices = new();

    private bool _isEnabled = false;

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
    
    public void Setup(HighlightService highlightService, PathHandler pathHandler)
    {
        _highlightService = highlightService;
        _pathHandler = pathHandler;
    }

    public void StartRouteCreation()
    {
        _selectedVertices.Clear();
        _availableVertices.Clear();
        _highlightService.EnableHighlight(_pathHandler.Graph.Vertices);
        _isEnabled = true;
    }

    public void ExitRouteCreation()
    {
        _highlightService.DisableHighlight(_pathHandler.Graph.Vertices);
        _isEnabled = false;
    }
    
    private void GameInputOnLeftClickPressed(object sender, EventArgs e)
    {
        if (!_isEnabled) return;
        
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        if (_grid.GetGridObject(x, y)?.Model is not RoadCell roadCell) return;
        if (!roadCell.IsVertexPoint) return;
        
        TryAddVertexLocation(new Location(x, y));
        
        bool finished = IsRouteCreationProcessFinished();
        
        if(!finished){ RefreshHighlights(); }
        else
        {
            _highlightService.DisableHighlight(_availableVertices);
            OnRouteCreated?.Invoke(this, _pathHandler.GetPathFromRoute(_selectedVertices));
            
            _isEnabled = false;
        }
    }

    private void TryAddVertexLocation(Location location)
    {
        if (_selectedVertices.Count == 0)
        {
            _selectedVertices.Add(location);
            _availableVertices = GraphAlgorithms.GetReachableGraphFromBfsTable(_pathHandler.Graph,
                GraphAlgorithms.Bfs(location, _pathHandler.Graph)).Vertices;
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
        _highlightService.DisableHighlight(_pathHandler.Graph.Vertices);
        _highlightService.EnableHighlight(GetHighlightableVertices());
    }


    
}
