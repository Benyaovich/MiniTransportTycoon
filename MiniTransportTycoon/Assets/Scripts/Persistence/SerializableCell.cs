using System;
using JetBrains.Annotations;

[Serializable]
public class SerializableCell
{
    [CanBeNull] public SerializableLocation origin;
    public SerializeableSize size;
    public bool destroyable;
        
    public SerializableCell(Cell cell)
    {
        origin = new SerializableLocation(cell.Origin);
        size = new SerializeableSize(cell.Size);
        destroyable = cell.Destroyable;
    }
    
    public SerializableCell(){}
}
