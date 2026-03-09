using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayCornerDRCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/Corner/DR")]
public class TwoWayCornerDRCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayCornerDR(location);
    }

    public override Type CellType => typeof(TwoWayCornerDR);
}

