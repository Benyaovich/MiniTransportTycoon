using System;
using Model.Cells.Facility;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtractorBuildingCoalCellObjectTypeSO", menuName = "Cell/Facilities/ExtractorBuildingCoal")]
public class ExtractorBuildingCoalCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ExtractorBuildingCoal(location, prodInterval: 2f, destroyable: true);
    }

    public override Type CellType => typeof(ExtractorBuildingCoal);
}