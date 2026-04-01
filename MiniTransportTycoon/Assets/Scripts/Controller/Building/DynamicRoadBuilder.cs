using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller.Building
{
    public class DynamicRoadBuilder : MonoBehaviour
    {
         public static DynamicRoadBuilder Instance { get; private set; }
         [CanBeNull] private Location _startPoint;
         [CanBeNull] private Location _endPoint;
         private List<Location> _selectedLocations = new();
         private Grid<ModelGridObject> _grid;
         private bool _roadBuildingActive;
         private BuilderState _state = BuilderState.BeforeFirst;

         private void Awake()
         {
             Instance = this;
             _grid = GridManager.Instance!.Grid;
         }

         private void OnEnable()
         {
             GameInput.Instance.OnLeftClickPressed += InstanceOnLeftClickPressed;
         }

         private void OnDisable()
         {
             GameInput.Instance.OnLeftClickPressed -= InstanceOnLeftClickPressed;
         }

         private void InstanceOnLeftClickPressed(object sender, EventArgs e)
         {
             if (!_roadBuildingActive) return;
             if (Utils.IsPointerOverBlockingUI()) return;
             
             if (_state == BuilderState.BeforeFirst)
             {
                 _grid.GetXY(Utils.GetMouseWorldPosition().SV3(), out int x, out int y);
                 _startPoint = new Location(x, y);
                 _state = BuilderState.AfterFirst;
             }
             else if (_state == BuilderState.AfterFirst)
             {
                 _grid.GetXY(Utils.GetMouseWorldPosition().SV3(), out int x, out int y);
                 Location location = new Location(x, y);
                 
                 if (_startPoint == location) return;
                 
                 _endPoint = location;
                 MakeSelectedPointsList();
                 GridManager.Instance!.BuildOnLocations(_selectedLocations);
                 ResetRoadBuilder();
             }
         }

         private void MakeSelectedPointsList()
         {
             if (_startPoint == null || _endPoint == null) throw new Exception("A kezdő vagy a végpont NULL!");
             
             Location diff = _endPoint - _startPoint;
             
             for (int x = 0; x <= Math.Abs(diff.X); x++)
             {
                 _selectedLocations.Add(new Location(_startPoint.X + Math.Sign(diff.X) * x,_startPoint.Y));
             }

             _startPoint = _selectedLocations.Last();
             for (int y = 1; y <= Math.Abs(diff.Y); y++)
             {
                 _selectedLocations.Add(new Location(_startPoint!.X,_startPoint.Y + Math.Sign(diff.Y) * y));
             }
             
         }

         public void ResetRoadBuilder()
         {
             _startPoint = null;
             _endPoint = null;
             _selectedLocations.Clear();
             _state = BuilderState.BeforeFirst;
         }
         
         public void SetActive(bool active){ _roadBuildingActive = active; }
         
        private enum BuilderState
        {
            BeforeFirst,
            AfterFirst
        }
    }

}