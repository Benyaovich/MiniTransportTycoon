using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayCornerDLCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/Corner/DL")]
public class TwoWayCornerDLCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayCornerDL(location);
    }

    public override Type CellType => typeof(TwoWayCornerDL);
}
