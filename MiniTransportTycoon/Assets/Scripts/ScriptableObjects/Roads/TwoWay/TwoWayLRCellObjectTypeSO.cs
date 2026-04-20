using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayLRCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/TwoWayLR")]
public class TwoWayLRCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayLR(location);
    }

    public override Type CellType => typeof(TwoWayLR);
}
