#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualService
{
    private readonly Grid<ModelGridObject> _grid;
    private readonly Transform _parentTransform;
    private readonly Dictionary<Type, CellObjectTypeSO> _cellLookup;
    private readonly Dictionary<Location, Transform> _cellVisualDictionary = new();
    
    public CellVisualService(
        Grid<ModelGridObject> grid,
        IBuildingManager buildingManager,
        Transform parentTransform,
        Dictionary<Type, CellObjectTypeSO> cellLookup)
    {
        _grid = grid;
        _parentTransform = parentTransform;
        _cellLookup = cellLookup;
        
        buildingManager.OnModelChanged += BuildingManagerOnModelChanged;
    }

    private void BuildingManagerOnModelChanged(object sender, OnModelChangedEventArgs e)
    {
        if (e.Cell is null){ DestroyVisualOnModelOrigin(e.Location); }
        else { CreateVisualForCell(e.Cell); }
    }


    public void CreateVisualForCell(Cell cell)
    {
        ModelGridObject origin = _grid.GetGridObject(cell.Origin.X, cell.Origin.Y);
        _cellVisualDictionary.TryAdd(origin.Location, InstantiateCellPrefab(origin));
        LinkVisualToModel(origin);
    }

    public void DestroyVisualOnModelOrigin(Location location)
    {
        if (!_cellVisualDictionary.ContainsKey(location)) return;
        UnityEngine.Object.Destroy(_cellVisualDictionary[location].gameObject);
        _cellVisualDictionary.Remove(location);
    }

    private void LinkVisualToModel(ModelGridObject origin)
    {
        if (origin.Model == null || !_cellVisualDictionary.ContainsKey(origin.Location))
            return;

        switch (origin.Model)
        {
            case Forest forest:
                ForestVisual forestVisual = _cellVisualDictionary[origin.Location].GetComponent<ForestVisual>();
                forestVisual.Setup(forest);
                break;
            case RoadCell roadCell:
                RoadCellVisual roadCellVisual = _cellVisualDictionary[origin.Location].GetComponent<RoadCellVisual>();
                roadCellVisual.Setup(roadCell);
                break;
        }
    }

    private Transform InstantiateCellPrefab(ModelGridObject go)
    {
        CellObjectTypeSO cellObjectTypeSo = GetCellObjectTypeSoForCell(go.Model!);
        return UnityEngine.Object.Instantiate(
            cellObjectTypeSo.prefab,
            go.Grid.GetWorldPosition(go.Location.X, go.Location.Y).UVXZ3(),
            Quaternion.identity,
            _parentTransform);
    }

    private CellObjectTypeSO GetCellObjectTypeSoForCell(Cell value)
    {
        CellObjectTypeSO coCellObjectTypeSo = _cellLookup[value.GetType()];

        if (coCellObjectTypeSo is null)
        {
            Debug.LogError("CellObjectTypeSO doesnt exist.");
            throw new NullReferenceException();
        }

        return coCellObjectTypeSo;
    }
}