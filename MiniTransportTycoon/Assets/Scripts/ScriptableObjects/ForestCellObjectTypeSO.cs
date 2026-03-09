using System;
using Model.Cells.RoadCells;
using UnityEngine;

[CreateAssetMenu(fileName = "ForestSOCellObjectTypeSO", menuName = "Cell/Forest")]
public class ForestCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new Forest(location, growthInterval: 3);
    }

    public override Type CellType => typeof(Forest);
}