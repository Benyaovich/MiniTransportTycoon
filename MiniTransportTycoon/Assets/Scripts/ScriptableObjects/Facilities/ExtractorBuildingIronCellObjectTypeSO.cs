using System;
using Model.Cells.Facility;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtractorBuildingIronCellObjectTypeSO", menuName = "Cell/Facilities/ExtractorBuildingIron")]
public class ExtractorBuildingIronCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ExtractorBuildingIron(location, prodInterval: 2f, destroyable: true);
    }

    public override Type CellType => typeof(ExtractorBuildingIron);
}