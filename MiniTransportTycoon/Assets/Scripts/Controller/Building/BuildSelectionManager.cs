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
    public int CurrentRotationDegrees { get; private set; }
    public Size CurrentSelectionSize { get; private set; } = new Size(1, 1);
    
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
        Keyboard? keyboard = Keyboard.current;
        if (keyboard == null) return;

        HandleRotationInput(keyboard);

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            SelectBusStopObjectTypeSo();
        }
        else if (keyboard.digit2Key.wasPressedThisFrame)
        {
            SelectDynamicRoadObjectTypeSo();
        }
        else if (keyboard.digit3Key.wasPressedThisFrame)
        {
            ClearSelectedObjectType();
        }
        else if (keyboard.digit4Key.wasPressedThisFrame)
        {
            CycleSelection(buildingCellObjectTypeSos!);
            InvokeBuildingSelected();
        }
    }

    private void HandleRotationInput(Keyboard keyboard)
    {
        if (SelectedObjectType == null) return;
        if (SelectedObjectType.CellType == typeof(DynamicRoadCell)) return;
        if (!keyboard.rKey.wasPressedThisFrame) return;

        RotateCurrentPlacement90();
    }

    public void RotateCurrentPlacement90()
    {
        CurrentRotationDegrees += 90;
        if (CurrentRotationDegrees >= 360)
        {
            CurrentRotationDegrees = 0;
        }
    }
    
    public void ClearSelectedObjectType()
    {
        SelectedObjectType = null;
        CurrentRotationDegrees = 0;
        CurrentSelectionSize = new Size(1, 1);
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
        if (cellObjectTypeSo.CellType == typeof(DynamicRoadCell)){
            CurrentSelectionSize = new Size(1, 1);
        }
        else{
            CurrentSelectionSize = cellObjectTypeSo.Create(new Location(0, 0)).Size;
        }
        RaiseSelectedObjectChanged();
    }

    public void SelectDynamicRoadObjectTypeSo()
    {
        CurrentRotationDegrees = 0;
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
