using System;
using System.Collections.Generic;
using Controller;
using Model.Enumerations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace View
{
    public class VehicleVisual : MonoBehaviour
    {
        [SerializeField] private Transform currentCell;
        [SerializeField] private bool debugMode = false;

        [Header("Offsets")]
        [SerializeField] private Vector3 upOffset = new Vector3(1.5f, 0, 3f);
        [SerializeField] private Vector3 downOffset = new Vector3(-1.5f, 0, -3f);
        [SerializeField] private Vector3 leftOffset = new Vector3(-3f, 0, 1.5f);
        [SerializeField] private Vector3 rightOffset = new Vector3(3f, 0, -1.5f);
        
        [Header("Testing")]
        [SerializeField] private Vector3 turningOffset = new Vector3(0.01f, 0, 0);
        
        private Vehicle _vehicle;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _nextPosition;
        private Vector3 _pivotPosition;
        private float _elapsedTime;
        private Vector3 _directionOffset;
        private MoveVisualMode _state = MoveVisualMode.Idle;
        
        
        

        public void Setup(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _vehicle.OnMove += VehicleOnMove;
            _vehicle.OnRouteSet += VehicleOnRouteSet;

        }

        private void OnDisable()
        {
            _vehicle.OnMove -= VehicleOnMove;
            _vehicle.OnRouteSet -= VehicleOnRouteSet;
        }

        private void Update()
        {
            if (_vehicle?.Route == null) return;
            if (_state == MoveVisualMode.Idle) return;

            DebugMode();
            
            _elapsedTime += Time.deltaTime;
            float routePercentage = Mathf.Clamp01(_elapsedTime / _vehicle.MoveSpeed);
            
            if (_state == MoveVisualMode.Straight)
            {
                transform.position = Vector3.Lerp(_previousPosition, _currentPosition, routePercentage);
            }
            else if (_state == MoveVisualMode.Turn)
            {
                transform.position = GetQuadraticBezierPoint(routePercentage, _previousPosition,
                    _pivotPosition, _currentPosition);
                Vector3 tangent = EvaluateQuadraticBezierTangent(routePercentage, _previousPosition,
                    _pivotPosition, _currentPosition);
                tangent.y = 0f;
                if (tangent.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(tangent);
            }

            if (_elapsedTime >= _vehicle.MoveSpeed)
            {
                _state = MoveVisualMode.Idle;
                _elapsedTime = 0;
                RotateBaseOnDirection();
            }
        }

        private void VehicleOnMove(object sender, Vehicle vehicle)
        {
            UpdateVisual();
        }

        private void VehicleOnRouteSet(object sender, EventArgs e)
        {
            RotateBaseOnDirection();
            UpdateVisual();
            transform.position = _currentPosition;
        }

        private void UpdateVisual()
        {
            if (_vehicle?.Route == null) return;
            
            OffsetBaseOnDirection();

            _elapsedTime = 0f;

            _previousPosition = _currentPosition;
            _currentPosition = GetWorldPosition(_vehicle.CurrentLocation);
            _nextPosition = GetWorldPosition(GetNextCellLocation());
            
            if (_vehicle.Route.IsTurning)
            {
                CalculatePivotPosition();
                _state = MoveVisualMode.Turn;
                return;
            }
            _state = MoveVisualMode.Straight;
        }

        private Location GetNextCellLocation()
        {
            return _vehicle.CurrentLocation + _vehicle.Route!.CurrentDirection;
        }

        private Vector3 GetWorldPosition(Location location)
        {
            return new Vector3(location.X, 0, location.Y) * _vehicle.Grid.CellSize
                   + new Vector3(_vehicle.Grid.CellSize / 2, 0, _vehicle.Grid.CellSize / 2)
                   + _directionOffset;
        }

        private void CalculatePivotPosition()
        {
            if (_vehicle?.Route == null) return;
            if (_vehicle.Route.TurningDirection == Direction.Left)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Up)
                    { _pivotPosition = _currentPosition + new Vector3(4.5f, 0, 0); }
                else if (_vehicle.Route.PreviousDirection == Direction.Down)
                    { _pivotPosition = _currentPosition + new Vector3(1.5f, 0, 0); }
            }

            else if (_vehicle.Route.TurningDirection == Direction.Right)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Down)
                    { _pivotPosition = _currentPosition + new Vector3(-4.5f, 0, 0); }
                else if (_vehicle.Route.PreviousDirection == Direction.Up)
                    { _pivotPosition = _currentPosition + new Vector3(-1.5f, 0, 0); }
            }
            
            else if (_vehicle.Route.TurningDirection == Direction.Down)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Right)
                    { _pivotPosition = _currentPosition + new Vector3(0, 0, 1.5f); }
                else if (_vehicle.Route.PreviousDirection == Direction.Left)
                    { _pivotPosition = _currentPosition + new Vector3(0, 0, 4.5f); }
                // else if (_vehicle.Route.PreviousDirection == Direction.Up)
                //     { _pivotPosition = _currentPosition + new Vector3(-1.5f, 0,  1.5f); }
            }
            
            else if (_vehicle.Route.TurningDirection == Direction.Up)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Right)
                    { _pivotPosition = _currentPosition + new Vector3(0, 0, -4.5f); }
                else if (_vehicle.Route.PreviousDirection == Direction.Left)
                    { _pivotPosition = _currentPosition + new Vector3(0, 0, -1.5f); }
                // else if (_vehicle.Route.PreviousDirection == Direction.Down)
                // { _pivotPosition = _currentPosition + new Vector3(-1.5f, 0,  -4.5f); }
            }
            
            
            
            
        }
        
        Vector3 GetQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 +
                   2 * u * t * p1 +
                   t * t * p2;
        }
        
        private Vector3 EvaluateQuadraticBezierTangent( float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
        }

        private void RotateBaseOnDirection()
        {
            if (_vehicle.Route == null) return;

            Direction dir = _vehicle.Route.CurrentDirection;

            if (dir == Direction.Up) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (dir == Direction.Down) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (dir == Direction.Left) transform.rotation = Quaternion.Euler(0, -90, 0);
            else if (dir == Direction.Right) transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        private void OffsetBaseOnDirection()
        {
            if (_vehicle.Route == null) return;

            
            Direction dir = _vehicle.Route.CurrentDirection;
            if (dir == Direction.Up) _directionOffset = new Vector3(1.5f, 0, 3f);
            else if (dir == Direction.Down) _directionOffset = new Vector3(-1.5f, 0, -3f);
            else if (dir == Direction.Left) _directionOffset = new Vector3(-3f, 0, 1.5f);
            else if (dir == Direction.Right) _directionOffset = new Vector3(3f, 0, -1.5f);
        }

        private void DebugMode()
        {
            if (!debugMode) return;
            
            
            if (_state == MoveVisualMode.Turn)
            {
                Debug.DrawLine(_previousPosition, _currentPosition, Color.blue);
                Debug.DrawLine(_currentPosition, _pivotPosition, Color.green);
                Debug.DrawLine(_pivotPosition, _nextPosition, Color.red);
            }
            else
            {
                Debug.DrawLine(_previousPosition, _currentPosition, Color.blue);
                Debug.DrawLine(_currentPosition, _nextPosition, Color.red);
            }
            // HighlightManager.Instance.HighlightService.HighlightFor(new List<Location>(){_vehicle.CurrentLocation}, 2f);
        }

        private enum MoveVisualMode
        {
            Idle,
            Straight,
            Turn
        }
    }
}