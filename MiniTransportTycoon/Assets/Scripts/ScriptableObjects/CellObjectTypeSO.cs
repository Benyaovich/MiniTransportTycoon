using System;
using UnityEngine;

public abstract class CellObjectTypeSO : ScriptableObject
{
    public Transform prefab;
    public Transform visual;
    public abstract Cell Create(Location location);
    public abstract Type CellType { get; }
}
