using System;
using System.Collections.Generic;
using Model.Cells.Grid;
using Model.Cells.RoadCells;
using Model.Enumerations;
using UnityEngine;

namespace Controller.Building
{
    public class DynamicRoadVisualService : VisualServiceBase
    {
        public DynamicRoadVisualService(Grid<ModelGridObject> grid, DynamicRoadBuildingManager buildingManager,
            Transform parentTransform, Dictionary<Type, CellObjectTypeSO> cellLookup)
            : base(grid, parentTransform, cellLookup)
        {
            buildingManager.OnRoadCellBuilt += BuildingManagerOnRoadCellBuilt;
            buildingManager.OnRoadCellRefreshed += BuildingManagerOnRoadCellRefreshed;
            buildingManager.OnRoadCellDemolished += BuildingManagerOnRoadCellDemolished;
        }

        private void BuildingManagerOnRoadCellDemolished(object sender, RoadCell roadCell)
        {
            DestroyVisualOnModelOrigin(roadCell.Origin);
        }

        private void BuildingManagerOnRoadCellRefreshed(object sender, RoadCell roadCell)
        {
            DestroyVisualOnModelOrigin(roadCell.Origin);
            CreateVisual(roadCell);
        }

        private void BuildingManagerOnRoadCellBuilt(object sender, RoadCell roadCell)
        {
            CreateVisual(roadCell);
        }

        private void CreateVisual(RoadCell roadCell)
        {
            ModelGridObject origin = Grid.GetGridObject(roadCell.Origin.X, roadCell.Origin.Y);
            VisualDictionary.TryAdd(origin.Location, InstantiatePrefab(origin, GetDynamicRoadCellSo(roadCell)));
            LinkVisualToModel(origin);
        }

        private CellObjectTypeSO GetDynamicRoadCellSo(RoadCell roadCell)
        {
            List<Direction> dir = roadCell.Directions;
            int count = dir.Count;
            
            
            switch (count)
            {
                case 1 when dir[0] == Direction.Up || dir[0] == Direction.Down:
                    return CellLookup[typeof(TwoWayUD)];
                case 1 when dir[0] == Direction.Left || dir[0] == Direction.Right:
                    return CellLookup[typeof(TwoWayLR)];
                case 2 when dir.Contains(Direction.Up) && dir.Contains(Direction.Down):
                    return CellLookup[typeof(TwoWayUD)];
                case 2 when dir.Contains(Direction.Left) && dir.Contains(Direction.Right):
                    return CellLookup[typeof(TwoWayLR)];
                case 2 when dir.Contains(Direction.Up) && dir.Contains(Direction.Right):
                    return CellLookup[typeof(TwoWayCornerUR)];
                case 2 when dir.Contains(Direction.Right) && dir.Contains(Direction.Down):
                    return CellLookup[typeof(TwoWayCornerDR)];
                case 2 when dir.Contains(Direction.Down) && dir.Contains(Direction.Left):
                    return CellLookup[typeof(TwoWayCornerDL)];
                case 2 when dir.Contains(Direction.Left) && dir.Contains(Direction.Up):
                    return CellLookup[typeof(TwoWayCornerUL)];
                case 3 when dir.Contains(Direction.Up) && dir.Contains(Direction.Right) && dir.Contains(Direction.Down):
                    return CellLookup[typeof(ThreeWayURD)];
                case 3 when dir.Contains(Direction.Right) && dir.Contains(Direction.Down) && dir.Contains(Direction.Left):
                    return CellLookup[typeof(ThreeWayRDL)];
                case 3 when dir.Contains(Direction.Down) && dir.Contains(Direction.Left) && dir.Contains(Direction.Up):
                    return CellLookup[typeof(ThreeWayUDL)];
                case 3 when dir.Contains(Direction.Left) && dir.Contains(Direction.Up) && dir.Contains(Direction.Right):
                    return CellLookup[typeof(ThreeWayURL)];
                case 4:
                    return CellLookup[typeof(FourWay)];
                default:
                    return CellLookup[typeof(TwoWayUD)]; // count == 0
            }
        
        }
    }
}