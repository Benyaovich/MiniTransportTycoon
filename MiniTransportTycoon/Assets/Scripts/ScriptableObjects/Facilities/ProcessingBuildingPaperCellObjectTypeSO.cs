using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ProcessingBuildingPaperCellObjectTypeSO", menuName = "Cell/Facilities/ProcessingBuildingPaper")]
public class ProcessingBuildingPaperCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ProcessingBuildingPaper(location, prodInterval: 2f, destroyable: true);
    }

    public override Type CellType => typeof(ProcessingBuildingPaper);
}