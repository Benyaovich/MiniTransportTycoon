#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controller.Building
{
    public class CellVisualService : VisualServiceBase
    {
    
        public CellVisualService(Grid<ModelGridObject> grid, CellBuildingManager buildingManager,
            Transform parentTransform, Dictionary<Type, CellObjectTypeSO> cellLookup)
            : base(grid, parentTransform, cellLookup)
        {
            buildingManager.OnCellChanged += BuildingManagerOnCellChanged;
        }

        private void BuildingManagerOnCellChanged(object sender, OnModelChangedEventArgs cell)
        {
            if (cell.Cell is null){ DestroyVisualOnModelOrigin(cell.Location); }
            else { CreateVisualForCell(cell.Cell); }
        }
    
        private void CreateVisualForCell(Cell cell)
        {
            ModelGridObject origin = Grid.GetGridObject(cell.Origin.X, cell.Origin.Y);
            VisualDictionary.TryAdd(origin.Location, InstantiatePrefab(origin, GetCellObjectTypeSoForCell(origin.Model)));
            LinkVisualToModel(origin);
        }
   
    }
}