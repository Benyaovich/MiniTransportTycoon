using System;
using Model.Cells.Facility;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtractorBuildingWoodCellObjectTypeSO", menuName = "Cell/Facilities/ExtractorBuildingWood")]
public class ExtractorBuildingWoodCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ExtractorBuildingWood(location, prodInterval: 2f, destroyable: true);
    }

    public override Type CellType => typeof(ExtractorBuildingWood);
}