using UnityEngine;

[CreateAssetMenu(fileName = "CellObjectTypeSO", menuName = "Scriptable Objects/CellObjectTypeSO")]
public class CellObjectTypeSO : ScriptableObject
{
    public string nameOfCellType;
    public Transform prefab;
    public Transform visual;
}
