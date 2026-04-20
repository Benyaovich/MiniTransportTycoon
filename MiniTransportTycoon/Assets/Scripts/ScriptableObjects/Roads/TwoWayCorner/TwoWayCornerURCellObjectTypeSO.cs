using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayCornerURCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/Corner/UR")]
public class TwoWayCornerURCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayCornerUR(location);
    }

    public override Type CellType => typeof(TwoWayCornerUR);
}

