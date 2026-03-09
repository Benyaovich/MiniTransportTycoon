#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelectionManager
{
    public event EventHandler<Transform?>? OnSelectedObjectChanged;

    public CellObjectTypeSO? SelectedObjectType { get; private set; }

    public void CycleSelection(List<CellObjectTypeSO> cellObjectTypes)
    {
        if (cellObjectTypes.Count < 1)
        {
            throw new Exception("A listában nincsenek elemek.\nRakj bele CellObjectTypeSO-kat");
        }

        if (SelectedObjectType is null || !cellObjectTypes.Contains(SelectedObjectType))
        {
            SelectedObjectType = cellObjectTypes[0];
            RaiseSelectedObjectChanged();
            return;
        }

        int index = cellObjectTypes.IndexOf(SelectedObjectType);
        index = (index + 1) % cellObjectTypes.Count;
        SelectedObjectType = cellObjectTypes[index];

        RaiseSelectedObjectChanged();
    }

    private void RaiseSelectedObjectChanged()
    {
        OnSelectedObjectChanged?.Invoke(this, SelectedObjectType!.visual);
    }
}