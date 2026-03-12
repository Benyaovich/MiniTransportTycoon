using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FourWayCellObjectTypeSO", menuName = "Cell/Roads/FourWay")]
public class FourWayCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new FourWay(location);
    }

    public override Type CellType => typeof(FourWay);
}
