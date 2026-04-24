using System;

[Serializable]
public class SCell
{
    
    public SLocation origin;
    public SSize size;
    public bool destroyable;
        
    public SCell(Cell cell)
    {
        origin = new SLocation(cell.Origin);
        size = new SSize(cell.Size);
        destroyable = cell.Destroyable;
    }

    public SCell()
    {
        
    }
}
