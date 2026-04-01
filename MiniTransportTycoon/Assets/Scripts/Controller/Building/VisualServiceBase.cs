using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controller.Building
{
    public abstract class VisualServiceBase
    {
        protected readonly Grid<ModelGridObject> Grid;
        protected readonly Transform ParentTransform;
        protected readonly Dictionary<Type, CellObjectTypeSO> CellLookup;
        protected readonly Dictionary<Location, Transform> VisualDictionary = new();
        
        protected VisualServiceBase(
            Grid<ModelGridObject> grid,
            Transform parentTransform,
            Dictionary<Type, CellObjectTypeSO> cellLookup)
        {
            Grid = grid;
            ParentTransform = parentTransform;
            CellLookup = cellLookup;
        }
        
        protected void DestroyVisualOnModelOrigin(Location location)
        {
            if (!VisualDictionary.ContainsKey(location)) return;
            UnityEngine.Object.Destroy(VisualDictionary[location].gameObject);
            VisualDictionary.Remove(location);
        }
        
        protected Transform InstantiatePrefab(ModelGridObject gridObject, CellObjectTypeSO cellObjectTypeSo)
        {
            return UnityEngine.Object.Instantiate(
                cellObjectTypeSo.prefab,
                gridObject.Grid.GetWorldPosition(gridObject.Location.X, gridObject.Location.Y).UVXZ3(),
                Quaternion.identity,
                ParentTransform);
        }
        
        protected CellObjectTypeSO GetCellObjectTypeSoForCell(Cell value)
        {
            CellObjectTypeSO coCellObjectTypeSo = CellLookup[value.GetType()];

            if (coCellObjectTypeSo is null)
            {
                Debug.LogError("CellObjectTypeSO doesnt exist.");
                throw new NullReferenceException();
            }

            return coCellObjectTypeSo;
        }
        
        protected void LinkVisualToModel(ModelGridObject origin)
        {
            if (origin.Model == null || !VisualDictionary.ContainsKey(origin.Location))
                return;

            switch (origin.Model)
            {
                case Forest forest:
                    ForestVisual forestVisual = VisualDictionary[origin.Location].GetComponent<ForestVisual>();
                    forestVisual.Setup(forest);
                    break;
                case RoadCell roadCell:
                    RoadCellVisual roadCellVisual = VisualDictionary[origin.Location].GetComponent<RoadCellVisual>();
                    roadCellVisual.Setup(roadCell);
                    break;
            }
        }
    }
}