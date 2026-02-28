using UnityEngine;

public class Field : Cell
{
    public Field(Location origin, Size size = null, bool destroyable = true) : base(origin, size, destroyable)
    {
    }
}
