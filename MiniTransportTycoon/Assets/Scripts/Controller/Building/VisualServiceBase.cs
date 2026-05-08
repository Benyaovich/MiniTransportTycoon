using System;
using System.Collections.Generic;
using UnityEngine;
using View;

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
            Vector3 originWorldPosition = gridObject.Grid.GetWorldPosition(gridObject.Location.X, gridObject.Location.Y).UVXZ3();
            int rotationDegrees = gridObject.Model!.RotationDegrees;
            Vector3 placementPosition = Utils.GetRotatedPlacementPosition(
                originWorldPosition,
                gridObject.Model.Size,
                gridObject.Grid.CellSize,
                rotationDegrees);

            return UnityEngine.Object.Instantiate(
                cellObjectTypeSo.prefab,
                placementPosition,
                Quaternion.Euler(0f, rotationDegrees, 0f),
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
                case Facility facility:
                    FacilityVisual facilityVisual = VisualDictionary[origin.Location].GetComponent<FacilityVisual>();
                    facilityVisual.Setup(facility);
                    break;
                case BusStop busStop:
                    BusStopVisual busStopVisual = VisualDictionary[origin.Location].GetComponent<BusStopVisual>();
                    busStopVisual.Setup(busStop);
                    break;
            }
        }
    }
}
