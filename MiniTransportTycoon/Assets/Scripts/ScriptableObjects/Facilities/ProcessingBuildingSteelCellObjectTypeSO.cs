using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ProcessingBuildingSteelCellObjectTypeSO", menuName = "Cell/Facilities/ProcessingBuildingSteel")]
public class ProcessingBuildingSteelCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ProcessingBuildingSteel(location, prodInterval: 2f, destroyable: true);
    }

    public override Type CellType => typeof(ProcessingBuildingSteel);
}