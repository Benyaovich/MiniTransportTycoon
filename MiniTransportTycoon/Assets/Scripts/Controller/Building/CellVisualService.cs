#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualService
{
    private readonly Grid<GridObject> _grid;
    private readonly Transform _parentTransform;
    private readonly Dictionary<Type, CellObjectTypeSO> _cellLookup;
    
    
    public CellVisualService(
        Grid<GridObject> grid,
        Transform parentTransform,
        Dictionary<Type, CellObjectTypeSO> cellLookup)
    {
        _grid = grid;
        _parentTransform = parentTransform;
        _cellLookup = cellLookup;
    }
    
    

    public void CreateVisualForCell(Cell cell)
    {
        GridObject origin = _grid.GetGridObject(cell.Origin.X, cell.Origin.Y);
        origin.SetVisual(InstantiateCellPrefab(origin));
        LinkVisualToModel(origin);
    }

    public void DestroyVisualOnModelOrigin(GridObject go)
    {
        GridObject origin = _grid.GetGridObject(go.Model!.Origin.X, go.Model.Origin.Y);
        if (origin.Visual is null) return;
        UnityEngine.Object.Destroy(origin.Visual.gameObject);
    }

    private void LinkVisualToModel(GridObject origin)
    {
        if (origin.Model == null || origin.Visual == null)
            return;

        switch (origin.Model)
        {
            case Forest forest:
                ForestVisual forestVisual = origin.Visual.GetComponent<ForestVisual>();
                forestVisual.Setup(forest);
                break;
            case RoadCell roadCell:
                RoadCellVisual roadCellVisual = origin.Visual.GetComponent<RoadCellVisual>();
                roadCellVisual.Setup(roadCell);
                break;
        }
    }

    private Transform InstantiateCellPrefab(GridObject go)
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