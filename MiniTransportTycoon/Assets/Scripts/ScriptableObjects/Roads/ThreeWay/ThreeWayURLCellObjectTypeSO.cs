using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeWayURLCellObjectTypeSO", menuName = "Cell/Roads/ThreeWay/URL")]
public class ThreeWayURLCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ThreeWayURL(location);
    }

    public override Type CellType => typeof(ThreeWayURL);
}
