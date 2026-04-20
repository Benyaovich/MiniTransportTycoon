using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DynamicRoadCellSO", menuName = "Cell/Roads/DynamicRoadCellSO")]
public class DynamicRoadCellSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        throw new Exception("Dynamic road cell can only be created by DynamicRoadCellManager.");
    }

    public override Type CellType => typeof(DynamicRoadCell);
}
