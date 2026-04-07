using System;
using Model.Enumerations;
using UnityEngine;

namespace View
{
    public class VehicleVisual : MonoBehaviour
    {
        [SerializeField] private Transform currentCell;
        [SerializeField] private bool debugMode = false;

        [Header("Direction Offsets")]
        [SerializeField] private Vector3 upDirectionOffset = new Vector3(3f, 0, 3f);
        [SerializeField] private Vector3 downDirectionOffset = new Vector3(-0.5f, 0, -3f);
        [SerializeField] private Vector3 leftDirectionOffset = new Vector3(-3f, 0, 3f);
        [SerializeField] private Vector3 rightDirectionOffset = new Vector3(3f, 0, -0.5f);
        
        
        [Header("Testing")]
        [SerializeField] private Vector3 testingOffset1 = new Vector3(0.01f, 0, 0);
        [SerializeField] private Vector3 testingOffset2 = new Vector3(0.01f, 0, 0);
        
        private Vehicle _vehicle;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _nextPosition;
        private Vector3 _pivotPosition1;
        private Vector3 _pivotPosition2;
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
            else if (_state == MoveVisualMode.Turn90)
            {
                transform.position = GetQuadraticBezierPoint(routePercentage, _previousPosition,
                    _pivotPosition1, _currentPosition);
                Vector3 tangent = EvaluateQuadraticBezierTangent(routePercentage, _previousPosition,
                    _pivotPosition1, _currentPosition);
                tangent.y = 0f;
                if (tangent.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(tangent);
            }
            else if (_state == MoveVisualMode.Turn180)
            {
                transform.position = GetCubicBezierPoint(routePercentage, _previousPosition, _pivotPosition1,
                    _pivotPosition2, _currentPosition);
                Vector3 tangent = EvaluateCubicBezierTangent(routePercentage, _previousPosition, _pivotPosition1,
                    _pivotPosition2, _currentPosition);
                tangent.y = 0f;
                if (tangent.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(tangent);
            }

            if (_elapsedTime >= _vehicle.MoveSpeed)
            {
                _state = MoveVisualMode.Idle;
                _elapsedTime = 0;
                RotateBasedOnDirection();
            }
        }

        private void VehicleOnMove(object sender, Vehicle vehicle)
        {
            UpdateVisual();
        }

        private void VehicleOnRouteSet(object sender, EventArgs e)
        {
            RotateBasedOnDirection();
            UpdateVisual();
            transform.position = _currentPosition;
        }

        private void UpdateVisual()
        {
            if (_vehicle?.Route == null) return;
            
            OffsetBasedOnDirection();

            _elapsedTime = 0f;

            _previousPosition = _currentPosition;
            _currentPosition = GetWorldPosition(_vehicle.CurrentLocation);
            _nextPosition = GetWorldPosition(GetNextCellLocation());
            
            if (_vehicle.Route.IsTurning)
            {
                if (_vehicle.Route.CurrentDirection == _vehicle.Route.PreviousDirection.Opposite())
                {
                    PivotForCubic();
                    _state = MoveVisualMode.Turn180;
                }
                else
                {
                    PivotForQuadratic();
                    _state = MoveVisualMode.Turn90;
                }
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

        private void PivotForQuadratic()
        {
            if (_vehicle?.Route == null) return;
            if (_vehicle.Route.TurningDirection == Direction.Left)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Up)
                    { _pivotPosition1 = _currentPosition + new Vector3(4.5f, 0, 0); }
                else if (_vehicle.Route.PreviousDirection == Direction.Down)
                    { _pivotPosition1 = _currentPosition + new Vector3(1.5f, 0, 0); }
            }

            else if (_vehicle.Route.TurningDirection == Direction.Right)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Down)
                    { _pivotPosition1 = _currentPosition + new Vector3(-4.5f, 0, 0); }
                else if (_vehicle.Route.PreviousDirection == Direction.Up)
                    { _pivotPosition1 = _currentPosition + new Vector3(-1.5f, 0, 0); }
            }
            
            else if (_vehicle.Route.TurningDirection == Direction.Down)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Right)
                    { _pivotPosition1 = _currentPosition + new Vector3(0, 0, 1.5f); }
                else if (_vehicle.Route.PreviousDirection == Direction.Left)
                    { _pivotPosition1 = _currentPosition + new Vector3(0, 0, 4.5f); }
            }
            
            else if (_vehicle.Route.TurningDirection == Direction.Up)
            {
                if (_vehicle.Route.PreviousDirection == Direction.Left)
                    { _pivotPosition1 = _currentPosition + new Vector3(0, 0, -1.5f); }
                else if (_vehicle.Route.PreviousDirection == Direction.Right)
                    { _pivotPosition1 = _currentPosition + new Vector3(0, 0, -4.5f); }
            }
            
        }
        
        private void PivotForCubic()
        {
            if (_vehicle?.Route == null) return;
            
            if (_vehicle.Route.TurningDirection == Direction.Down &&
                _vehicle.Route.PreviousDirection == Direction.Up)
            {
                _pivotPosition1 = _currentPosition + new Vector3(3.5f,0,5);
                _pivotPosition2 = _currentPosition + new Vector3(0,0,5);
            }
            else if (_vehicle.Route.TurningDirection == Direction.Up &&
                     _vehicle.Route.PreviousDirection == Direction.Down)
            {
                _pivotPosition1 = _currentPosition + new Vector3(-3.5f,0,-5);
                _pivotPosition2 = _currentPosition + new Vector3(0,0,-5);
            }
            else if (_vehicle.Route.TurningDirection == Direction.Left &&
                     _vehicle.Route.PreviousDirection == Direction.Right)
            {
                _pivotPosition1 = _currentPosition + new Vector3(6,0,-3.5f);
                _pivotPosition2 = _currentPosition + new Vector3(6,0,0);
            }
            else if (_vehicle.Route.TurningDirection == Direction.Right &&
                     _vehicle.Route.PreviousDirection == Direction.Left)
            {
                _pivotPosition1 = _currentPosition + new Vector3(-6,0,3.5f);
                _pivotPosition2 = _currentPosition + new Vector3(-6,0,0);
            }
            
        }


        #region Quadratic Bezier
        
           
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
        
        #endregion
        
        #region Cubic Bezier
        private Vector3 GetCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1f - t;

            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return uuu * p0 +
                   3f * uu * t * p1 +
                   3f * u * tt * p2 +
                   ttt * p3;
        }

        private Vector3 EvaluateCubicBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1f - t;

            return 3f * u * u * (p1 - p0) +
                   6f * u * t * (p2 - p1) +
                   3f * t * t * (p3 - p2);
        }
        
        #endregion

        private void RotateBasedOnDirection()
        {
            if (_vehicle.Route == null) return;

            Direction dir = _vehicle.Route.CurrentDirection;

            if (dir == Direction.Up) transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (dir == Direction.Down) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (dir == Direction.Left) transform.rotation = Quaternion.Euler(0, -90, 0);
            else if (dir == Direction.Right) transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        private void OffsetBasedOnDirection()
        {
            if (_vehicle.Route == null) return;

            
            Direction dir = _vehicle.Route.CurrentDirection;
            if (dir == Direction.Up) _directionOffset = upDirectionOffset;
            else if (dir == Direction.Down) _directionOffset = downDirectionOffset;
            else if (dir == Direction.Left) _directionOffset = leftDirectionOffset;
            else if (dir == Direction.Right) _directionOffset = rightDirectionOffset;
        }

        private void OnDrawGizmos()
        {
            if (!debugMode) return;
            if (_state == MoveVisualMode.Turn90)
            {
                Gizmos.DrawSphere(_previousPosition, 1f);
                Gizmos.DrawSphere(_pivotPosition1, 1f);
                Gizmos.DrawSphere(_currentPosition, 1f);
            }
            else if (_state == MoveVisualMode.Turn180)
            {
                Gizmos.DrawSphere(_previousPosition, 1f);
                Gizmos.DrawSphere(_pivotPosition1, 1f);
                Gizmos.DrawSphere(_pivotPosition2, 1f);
                Gizmos.DrawSphere(_currentPosition, 1f);
            }
        }

        private void DebugMode()
        {
            if (!debugMode) return;
            
            
            if (_state == MoveVisualMode.Turn90)
            {
                Debug.DrawLine(_previousPosition, _pivotPosition1, Color.blue);
                Debug.DrawLine(_pivotPosition1, _currentPosition, Color.green);
                Debug.DrawLine(_currentPosition, _nextPosition, Color.red);
                
            }
            else if(_state == MoveVisualMode.Turn180)
            {
                Debug.DrawLine(_previousPosition, _pivotPosition1, Color.blue);
                Debug.DrawLine(_pivotPosition1, _pivotPosition2, Color.green);
                Debug.DrawLine(_pivotPosition2, _currentPosition, Color.red);
                Debug.DrawLine(_currentPosition, _nextPosition, Color.orange);
            }
            // HighlightManager.Instance.HighlightService.HighlightFor(new List<Location>(){_vehicle.CurrentLocation}, 2f);
        }

        private enum MoveVisualMode
        {
            Idle,
            Straight,
            Turn90,
            Turn180
        }
    }
}