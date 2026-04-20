using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeWayUDLCellObjectTypeSO", menuName = "Cell/Roads/ThreeWay/UDL")]
public class ThreeWayUDLCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ThreeWayUDL(location);
    }

    public override Type CellType => typeof(ThreeWayUDL);
}
