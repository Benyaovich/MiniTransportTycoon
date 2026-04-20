using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Controller.Building
{
    public class DynamicRoadBuilder : MonoBehaviour
    {
        [CanBeNull] public event EventHandler<List<Location>> OnEndPointChanged;
        [CanBeNull] public event EventHandler OnReset;
         public static DynamicRoadBuilder Instance { get; private set; }
         [CanBeNull] private Location _startPoint;
         [CanBeNull] private Location _midPoint;
         [CanBeNull] private Location _endPoint;
         private List<Location> _selectedLocations = new();
         private Grid<ModelGridObject> _grid;
         private bool _roadBuildingActive;
         private BuilderState _state = BuilderState.BeforeFirst;

         public Grid<ModelGridObject> Grid => _grid;
         private void Awake()
         {
             Instance = this;
             _grid = GridManager.Instance!.Grid;
         }

         private void OnEnable()
         {
             GameInput.Instance.OnRightClickPressed += GameInputOnRightClickPressed;
             GameInput.Instance.OnLeftClickStarted += InstanceOnLeftClickStarted;
             GameInput.Instance.OnLeftClickCanceled += InstanceOnLeftClickCanceled;
         }

         private void OnDisable()
         {
             GameInput.Instance.OnRightClickPressed -= GameInputOnRightClickPressed;
             GameInput.Instance.OnLeftClickStarted -= InstanceOnLeftClickStarted;
             GameInput.Instance.OnLeftClickCanceled -= InstanceOnLeftClickCanceled;
         }

         private void Update()
         {
             if (!_roadBuildingActive) return;
             
             if (_state == BuilderState.AfterFirst)
             {
                 _grid.GetXY(Utils.GetMouseWorldPosition().SV3(), out int x, out int y);
                 Location location = new Location(x, y);

                 if (_midPoint == location) return;
                 
                 _midPoint = location;
                 InvokeEndPointChanged(MakeSelectedPointsList(_startPoint, _midPoint)
                     .Where(loc =>_grid.GetGridObject(loc)?.Model == null).ToList());
             }
         }

         private void GameInputOnRightClickPressed(object sender, EventArgs e)
         {
             // if (!_roadBuildingActive) return;
             // if (Utils.IsPointerOverBlockingUI()) return;
             //
             // if (_startPoint != null) { ResetRoadBuilder(); }
             // else{ GridManager.Instance!.BuildOnCurrentMousePosition(); }
             
         }
         
         private void InstanceOnLeftClickStarted(object sender, EventArgs e)
         {
             if (!_roadBuildingActive) return;
             if (Utils.IsPointerOverBlockingUI()) return;
             if (_state != BuilderState.BeforeFirst) return;
                 
             _grid.GetXY(Utils.GetMouseWorldPosition().SV3(), out int x, out int y);
             _startPoint = new Location(x, y);
             _state = BuilderState.AfterFirst;
         }
         
         private void InstanceOnLeftClickCanceled(object sender, EventArgs e)
         {
             if (!_roadBuildingActive) return;
             if (Utils.IsPointerOverBlockingUI()) return;
             if (_state != BuilderState.AfterFirst) return;
             
             _grid.GetXY(Utils.GetMouseWorldPosition().SV3(), out int x, out int y);
             Location location = new Location(x, y);

             if (_startPoint == location)
             {
                 ResetRoadBuilder();
                 return;
             }
                 
             _endPoint = location;
             _selectedLocations = MakeSelectedPointsList(_startPoint, _endPoint);
             GridManager.Instance!.BuildOnLocations(_selectedLocations);
             ResetRoadBuilder();
         }


         private List<Location> MakeSelectedPointsList(Location start, Location end)
         {
             if (start == null || end == null) throw new Exception("A kezdő vagy a végpont NULL!");
             
             Location diff = end - start;
             List<Location> locations = new();
             for (int x = 0; x <= Math.Abs(diff.X); x++)
             {
                 locations.Add(new Location(start.X + Math.Sign(diff.X) * x,start.Y));
             }

             start = locations.Last();
             for (int y = 1; y <= Math.Abs(diff.Y); y++)
             {
                 locations.Add(new Location(start!.X,start.Y + Math.Sign(diff.Y) * y));
             }

             return locations;
         }

         public void ResetRoadBuilder()
         {
             _startPoint = null;
             _endPoint = null;
             _selectedLocations.Clear();
             _state = BuilderState.BeforeFirst;
             InvokeOnReset();
         }
         
         public void SetActive(bool active){ _roadBuildingActive = active; }
         
         private void InvokeEndPointChanged(List<Location> locations)
         {
             OnEndPointChanged?.Invoke(this, locations);
         }

         private void InvokeOnReset()
         {
             OnReset?.Invoke(this, EventArgs.Empty);
         }
         
        public enum BuilderState
        {
            BeforeFirst,
            AfterFirst
        }
    }
}