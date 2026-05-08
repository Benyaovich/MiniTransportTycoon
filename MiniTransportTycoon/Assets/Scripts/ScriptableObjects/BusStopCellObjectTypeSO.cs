using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BusStopCellObjectTypeSO", menuName = "Cell/BusStop")]
public class BusStopCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new BusStop(location);
    }

    public override Type CellType => typeof(BusStop);
}
