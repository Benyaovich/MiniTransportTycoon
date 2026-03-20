using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BusStopCellObjectTypeSO", menuName = "Cell/BusStop")]
public class BusStopCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new BusStop(location, interval: 2);
    }

    public override Type CellType => typeof(BusStop);
}
