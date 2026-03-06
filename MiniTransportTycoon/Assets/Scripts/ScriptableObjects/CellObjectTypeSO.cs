using UnityEngine;

[CreateAssetMenu(fileName = "CellObjectTypeSO", menuName = "Scriptable Objects/CellObjectTypeSO")]
public class CellObjectTypeSO : ScriptableObject
{
    public BuildingTypes buildingType;
    public Transform prefab;
    public Transform visual;
}
