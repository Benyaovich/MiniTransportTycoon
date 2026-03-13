using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SmallCityCellObjectTypeSO", menuName = "Cell/Cities/SmallCity")]
public class SmallCityCellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new SmallCity(location);
    }

    public override Type CellType => typeof(SmallCity);
}
