using System;
using System.Xml.Schema;
using Model.Cells.Cities.Houses;

[Serializable]
public class SCell
{
    
    public SLocation origin;
    public SSize size;
    public int rotationDegrees;
        
    public SCell(Cell cell)
    {
        origin = new SLocation(cell.Origin);
        size = new SSize(cell.Size);
        rotationDegrees = cell.RotationDegrees;
    }

    public SCell()
    {
        
    }
}

[Serializable]
public class SResidentialBuilding1 : SCell
{
    public SResidentialBuilding1(ResidentialBuilding1 residentialBuilding1) : base(residentialBuilding1) { }
    public SResidentialBuilding1() { }
}

[Serializable]
public class SResidentialBuilding2 : SCell
{
    public SResidentialBuilding2(ResidentialBuilding2 residentialBuilding2) : base(residentialBuilding2) { }
    public SResidentialBuilding2() { }
}
