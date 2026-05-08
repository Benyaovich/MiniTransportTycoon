using System;
using Model.Cells.Cities.Houses;
using UnityEngine;

[CreateAssetMenu(fileName = "ResidentialBuilding1CellObjectTypeSO", menuName = "Cell/Cities/Houses/ResidentialBuildings/1")]
public class ResidentialBuilding1CellObjectTypeSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new ResidentialBuilding1(location, destroyable: true);
    }

    public override Type CellType => typeof(ResidentialBuilding1);
}
