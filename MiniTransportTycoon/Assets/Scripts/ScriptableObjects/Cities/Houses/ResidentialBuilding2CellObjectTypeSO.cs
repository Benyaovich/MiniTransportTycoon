using System;
using Model.Cells.Cities.Houses;
using UnityEngine;

[CreateAssetMenu(fileName = "ResidentialBuilding2CellObjectTypeSO", menuName = "Cell/Cities/Houses/ResidentialBuildings/2")]
public class ResidentialBuilding2CellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ResidentialBuilding2(location);
    }

    public override Type CellType => typeof(ResidentialBuilding2);
}
