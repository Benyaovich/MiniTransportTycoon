using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoWayUDCellObjectTypeSO", menuName = "Cell/Roads/TwoWay/TwoWayUD")]
public class TwoWayUDCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TwoWayUD(location);
    }
    
    public override Type CellType => typeof(TwoWayUD);
}
