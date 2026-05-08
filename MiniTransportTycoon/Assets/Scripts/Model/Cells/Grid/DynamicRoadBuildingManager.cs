#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;

namespace Model.Cells.Grid
{
    public class DynamicRoadBuildingManager : BuildingManagerBase
    {
        public event EventHandler<RoadCell>? OnRoadCellBuilt; 
        public event EventHandler<RoadCell>? OnRoadCellDemolished; 
        public event EventHandler<RoadCell>? OnRoadCellRefreshed; 

        public DynamicRoadBuildingManager(IGrid<ModelGridObject> grid) : base(grid)
        {
        }
        

        public void TryBuildRoad(Location location)
        {
            Dictionary<Direction, Cell?> neighbours = GetNeighbours(location);
            DynamicRoadCell roadCell = new DynamicRoadCell(location, neighbours);
            
            List<Location>? gridPositionList = roadCell.GetGridPositionList();
            if (!CheckIfCanBuild(gridPositionList)) return;
            
            
            var dynamicRoadCells = GetNeighboursAsDynamicRoadList(neighbours);
            
            SetModelsValueInGridObjects(roadCell, gridPositionList);
            
            UpdateNeighbours(dynamicRoadCells);
            RefreshNeighbours(dynamicRoadCells);
            
            PlayerState.Instance.SpendMoney(roadCell.Price);
            
            InvokeRoadCellBuilt(roadCell);
        }

        public bool CanBuildRoad(Location location)
        {
            Dictionary<Direction, Cell?> neighbours = GetNeighbours(location);
            DynamicRoadCell roadCell = new DynamicRoadCell(location, neighbours);
            return CheckIfCanBuild(roadCell.GetGridPositionList());
        }


        public void TryDemolishRoad(Location location)
        {
            ModelGridObject go = Grid.GetGridObject(location.X, location.Y);

            if (go.Model is not RoadCell roadCell) return;
            if (!roadCell.Destroyable) return;
            
            List<Location> gridPositionList = roadCell.GetGridPositionList();
            go.ClearModel();
            ClearModelFromGridObjects(gridPositionList);
            
            PlayerState.Instance.SpendMoney(GetCellRemovePrice(roadCell));
            InvokeRoadCellDemolished(roadCell);
            
            List<DynamicRoadCell> neighbours = GetNeighboursAsDynamicRoadList(GetNeighbours(roadCell.Origin));
            UpdateNeighboursWhenRemoving(neighbours);

        }

        

        private void UpdateNeighboursWhenRemoving(List<DynamicRoadCell> dynamicRoadCells)
        {
            foreach(DynamicRoadCell dynamicRoadCell in dynamicRoadCells)
            {
                InvokeRoadCellDemolished(dynamicRoadCell);
            }
            UpdateNeighbours(dynamicRoadCells);
            foreach(DynamicRoadCell dynamicRoadCell in dynamicRoadCells)
            {
                InvokeRoadCellBuilt(dynamicRoadCell);
            }
        }

        public void RefreshRoad(DynamicRoadCell roadCell)
        {
            var neighbours = GetNeighbours(roadCell.Origin);
            List<DynamicRoadCell> dynamicRoadCells = GetNeighboursAsDynamicRoadList(neighbours);
            
            roadCell.Refresh(neighbours);
            UpdateNeighbours(dynamicRoadCells);
            
            RefreshNeighbours(dynamicRoadCells);
            InvokeRoadCellRefreshed(roadCell);
        }

        private void UpdateNeighbours(List<DynamicRoadCell> dynamicRoadCells)
        {
            foreach(DynamicRoadCell dynamicRoadCell in dynamicRoadCells)
            {
                dynamicRoadCell.Refresh(GetNeighbours(dynamicRoadCell.Origin));
            }
        }

        private void RefreshNeighbours(List<DynamicRoadCell> dynamicRoadCells)
        {
            foreach(DynamicRoadCell dynamicRoadCell in dynamicRoadCells)
            {
                InvokeRoadCellRefreshed(dynamicRoadCell);
            }
        }


        
        private List<DynamicRoadCell> GetNeighboursAsDynamicRoadList(Dictionary<Direction, Cell?> neighbours)
        {
            List<DynamicRoadCell> dynamicRoadCells = new();
            foreach(Direction direction in neighbours.Keys)
            {
                if (neighbours[direction] is not DynamicRoadCell neighbourRoadCell) continue;
                dynamicRoadCells.Add(neighbourRoadCell);
            }
            return dynamicRoadCells;
        }

        private Dictionary<Direction, Cell?> GetNeighbours(Location location)
        {
            Dictionary<Direction, Cell?> neighbours = new();
            neighbours.TryAdd(Direction.Up, Grid.GetGridObject(location + Direction.Up)?.Model);
            neighbours.TryAdd(Direction.Down, Grid.GetGridObject(location + Direction.Down)?.Model);
            neighbours.TryAdd(Direction.Left, Grid.GetGridObject(location + Direction.Left)?.Model);
            neighbours.TryAdd(Direction.Right, Grid.GetGridObject(location + Direction.Right)?.Model);
            return neighbours;
        }
        
        private void InvokeRoadCellBuilt(RoadCell roadCell)
        {
            OnRoadCellBuilt?.Invoke(this, roadCell);
        }
    
        private void InvokeRoadCellDemolished(RoadCell roadCell)
        {
            OnRoadCellDemolished?.Invoke(this, roadCell);
        }
        
        private void InvokeRoadCellRefreshed(RoadCell roadCell)
        {
            OnRoadCellRefreshed?.Invoke(this, roadCell);
        }
    }
}
