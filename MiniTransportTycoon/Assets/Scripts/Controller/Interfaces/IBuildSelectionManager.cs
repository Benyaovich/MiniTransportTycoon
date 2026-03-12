
#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildSelectionManager
{
    public event EventHandler<Transform?>? OnSelectedObjectChanged;
    public CellObjectTypeSO? SelectedObjectType { get; }
    public void CycleSelection(List<CellObjectTypeSO> cellObjectTypes);
}
