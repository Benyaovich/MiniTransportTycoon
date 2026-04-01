#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class BuildSelectionManager : MonoBehaviour, IBuildSelectionManager
{
    public static BuildSelectionManager Instance { get; private set; } = null!;
    public event EventHandler<Transform?>? OnSelectedObjectChanged;

    public CellObjectTypeSO? SelectedObjectType { get; private set; }

    private List<CellObjectTypeSO> _cellObjectTypeSos = new();
    public Dictionary<Type, CellObjectTypeSO> CellLookup { get; private set; } = new();
    
    [SerializeField] private List<CellObjectTypeSO>? buildingCellObjectTypeSos;
    [SerializeField] private List<CellObjectTypeSO>? roadCellObjectsTypeSos;
    [SerializeField] private CellObjectTypeSO? dynamicRoadCellObjectTypeSo;

    private void Awake()
    {
        Instance = this;
        CollectAllCellObjectTypeSosIntoASingleList();
        BuildLookup();
    }
    
    public void HandleBuildSelectionInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CycleSelection(buildingCellObjectTypeSos!);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetSelectedObjectType(dynamicRoadCellObjectTypeSo!);
        }
    }
    
    public void ClearSelectedObjectType()
    {
        SelectedObjectType = null;
        RaiseSelectedObjectChanged();
    }

    public void CycleSelection(List<CellObjectTypeSO> cellObjectTypes)
    {
        if (cellObjectTypes.Count < 1)
        {
            throw new Exception("A listában nincsenek elemek.\nRakj bele CellObjectTypeSO-kat");
        }

        if (SelectedObjectType is null || !cellObjectTypes.Contains(SelectedObjectType))
        {
            SetSelectedObjectType(cellObjectTypes[0]);
            return;
        }

        int index = cellObjectTypes.IndexOf(SelectedObjectType);
        index = (index + 1) % cellObjectTypes.Count;
        SetSelectedObjectType(cellObjectTypes[index]);
    }

    private void SetSelectedObjectType(CellObjectTypeSO cellObjectTypeSo)
    {
        SelectedObjectType = cellObjectTypeSo;
        RaiseSelectedObjectChanged();
    }
    
    private void CollectAllCellObjectTypeSosIntoASingleList()
    {
        _cellObjectTypeSos = new List<CellObjectTypeSO>();

        foreach (CellObjectTypeSO cellObjectTypeSo in buildingCellObjectTypeSos!)
        {
            _cellObjectTypeSos.Add(cellObjectTypeSo);
        }

        foreach (CellObjectTypeSO cellObjectTypeSo in roadCellObjectsTypeSos!)
        {
            _cellObjectTypeSos.Add(cellObjectTypeSo);
        }
    }
    
    private void BuildLookup()
    {
        CellLookup = new Dictionary<Type, CellObjectTypeSO>();
        
        foreach (var so in _cellObjectTypeSos)
        {
            CellLookup.Add(so.CellType, so);
        }
    }

    private void RaiseSelectedObjectChanged()
    {
        if (SelectedObjectType is null)
        {
            OnSelectedObjectChanged?.Invoke(this, null);
            return;
        }
        
        OnSelectedObjectChanged?.Invoke(this, SelectedObjectType.visual);
    }
}