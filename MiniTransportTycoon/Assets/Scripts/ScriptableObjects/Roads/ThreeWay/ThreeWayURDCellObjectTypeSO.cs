using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeWayURDCellObjectTypeSO", menuName = "Cell/Roads/ThreeWay/URD")]
public class ThreeWayURDCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ThreeWayURD(location);
    }

    public override Type CellType => typeof(ThreeWayURD);
}
