using System;
using Model.Cells.RoadCells;

public class CellFactory
{
    public Cell Create(CellObjectTypeSO cellObjectTypeSo, Location location)
    {
        switch (cellObjectTypeSo.buildingType)
        {
            case BuildingTypes.Forest:
                return new Forest(location, growthInterval: 3);
            case BuildingTypes.ProcessingBuildingSteel:
                return new ProcessingBuildingSteel(location, prodInterval: 2f, destroyable: true);
            case BuildingTypes.TwoWayUD:
                return new TwoWayUD(location);
            case BuildingTypes.TwoWayLR:
                return new TwoWayLR(location);
            case BuildingTypes.TwoWayCornerDL:
                return new TwoWayCornerDL(location);
            case BuildingTypes.TwoWayCornerDR:
                return new TwoWayCornerDR(location);
            case BuildingTypes.TwoWayCornerUL:
                return new TwoWayCornerUL(location);
            case BuildingTypes.TwoWayCornerUR:
                return new TwoWayCornerUR(location);
            default:
                throw new InvalidOperationException("Ismeretlen building type.");
        }
    }
}