#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSelectionManager : MonoBehaviour, IBuildSelectionManager
{
    public static BuildSelectionManager Instance { get; private set; } = null!;
    public event EventHandler<Transform?>? OnSelectedObjectChanged;
    public event EventHandler? OnDynamicRoadSelected;
    public event EventHandler? OnBuildingSelected; 

    public CellObjectTypeSO? SelectedObjectType { get; private set; }
    
    private List<CellObjectTypeSO> _cellObjectTypeSos = new();
    public Dictionary<Type, CellObjectTypeSO> CellLookup { get; private set; } = new();
    
    [SerializeField] private List<CellObjectTypeSO> buildingCellObjectTypeSos = null!;
    [SerializeField] private List<CellObjectTypeSO> roadCellObjectsTypeSos = null!;
    [SerializeField] private CellObjectTypeSO dynamicRoadCellObjectTypeSo = null!; 
    [SerializeField] private CellObjectTypeSO busStop = null!;

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
            InvokeBuildingSelected();
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelectDynamicRoadObjectTypeSo();
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

    public void SelectDynamicRoadObjectTypeSo()
    {
        SetSelectedObjectType(dynamicRoadCellObjectTypeSo);
        InvokeDynamicRoadSelected();
    }

    public void SelectBusStopObjectTypeSo()
    {
        SetSelectedObjectType(busStop);
        InvokeBuildingSelected();
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
    
    private void InvokeDynamicRoadSelected()
    {
        OnDynamicRoadSelected?.Invoke(this, EventArgs.Empty);
    }
    private void InvokeBuildingSelected()
    {
        OnBuildingSelected?.Invoke(this, EventArgs.Empty);
    }
}