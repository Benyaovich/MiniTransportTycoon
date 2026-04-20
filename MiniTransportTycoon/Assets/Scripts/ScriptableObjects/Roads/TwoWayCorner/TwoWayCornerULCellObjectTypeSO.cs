using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayCornerULCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/Corner/UL")]
public class TwoWayCornerULCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayCornerUL(location);
    }

    public override Type CellType => typeof(TwoWayCornerUL);
}

