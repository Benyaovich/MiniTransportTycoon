using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeWayRDLCellObjectTypeSO", menuName = "Cell/Roads/ThreeWay/RDL")]
public class ThreeWayRDLCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ThreeWayRDL(location);
    }

    public override Type CellType => typeof(ThreeWayRDL);
}
