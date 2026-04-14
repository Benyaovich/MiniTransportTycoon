using System;
using Model.Enumerations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace View
{
    public class VehicleVisual : MonoBehaviour
    {
        [SerializeField] private bool debugMode;
        [SerializeField] private float sideWaysOffset = 1.5f;
        [SerializeField] private float straightOffset = 3f;

        [SerializeField] private Vector3 rightTurnPivot1 = new Vector3(0,0,4);
        [SerializeField] private Vector3 rightTurnPivot2 = new Vector3(0,0,0);
        [SerializeField] private Vector3 rightTurnDestinationOffset = new Vector3(1,0,0);
        
        [SerializeField] private Vector3 leftTurnPivot1 = new Vector3(0,0,-7);
        [SerializeField] private Vector3 leftTurnPivot2 = new Vector3(-3,0,0);
        
        [SerializeField] private Vector3 uTurnFirstPartPivot1 = new Vector3(4,0,-1.5f);
        [SerializeField] private Vector3 uTurnFirstPartPivot2 = new Vector3(7f,0,-1f);
        [SerializeField] private Vector3 uTurnFirstPartDestinationOffset = new Vector3(8,0,1f);
        
        
        private Vehicle _vehicle;
        private Vector3 _cellCenter;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _pivotPosition1;
        private Vector3 _pivotPosition2;
        private float _elapsedTime;
        private MoveVisualMode _state = MoveVisualMode.Idle;
        
        
        

        public void Setup(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _cellCenter = new Vector3(_vehicle.Grid.CellSize / 2, 0, _vehicle.Grid.CellSize / 2);
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
            
            _elapsedTime += GameManager.Instance.DeltaTime;
            float routePercentage = Mathf.Clamp01(_elapsedTime / _vehicle.MoveSpeed);
            
            if (_state == MoveVisualMode.Straight)
            {
                transform.position = Vector3.Lerp(_previousPosition, _currentPosition, routePercentage);
            }
            else if (_state == MoveVisualMode.Turn180 ||
                     _state == MoveVisualMode.Turn90)
            {
                transform.position = GetCubicBezierPoint(routePercentage, _previousPosition, _pivotPosition1,
                    _pivotPosition2, _currentPosition);
                Vector3 tangent = EvaluateCubicBezierTangent(routePercentage, _previousPosition, _pivotPosition1,
                    _pivotPosition2, _currentPosition);
                tangent.y = 0f;
                if (tangent.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(tangent);
            }

            if (_elapsedTime >= _vehicle.MoveSpeed - 0.002)
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
            _currentPosition = GetCurrentCellCenter() + GetSideWaysOffset()
                                                      + GetStraightOffset(_vehicle.Route!.CurrentDirection.Opposite());
            transform.position = _currentPosition;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_vehicle?.Route == null) return;
            _elapsedTime = 0f;
            
            if (_vehicle.Route.IsTurning)
            {
                if (_vehicle.Route.Turns180happened)
                {
                    FirstUTurnSetup();
                    _state = MoveVisualMode.Turn180;
                }
                else if (_vehicle.Route.Turns180Finished)
                {
                    SecondUTurnSetup();
                    _state = MoveVisualMode.Turn180;
                }
                else
                {
                    if (_vehicle.Route.PreviousDirection.TurnRightClockwise() == _vehicle.Route.CurrentDirection)
                    {
                        RightTurnSetup();
                    }
                    else
                    {
                        LeftTurnSetup();
                    }
                    _state = MoveVisualMode.Turn90;
                }
                return;
            }
            
            WorldPositionsStraight();
            _state = MoveVisualMode.Straight;
        }
        

        private void FirstUTurnSetup()
        {
            float degree = GetTurnRotation();

            _previousPosition = _currentPosition;
            
            _pivotPosition1 = _previousPosition + uTurnFirstPartPivot1;
            _pivotPosition2 = _previousPosition + uTurnFirstPartPivot2;
            _currentPosition = _previousPosition + uTurnFirstPartDestinationOffset;
            
            _pivotPosition1 = RotateAroundPivot(_pivotPosition1, _previousPosition, degree);
            _pivotPosition2 = RotateAroundPivot(_pivotPosition2, _previousPosition, degree);
            _currentPosition = RotateAroundPivot(_currentPosition, _previousPosition, degree);
        }
        
        private void SecondUTurnSetup()
        {
            Vector3 a = GetCurrentCellCenter() - new Vector3(-5, 0, 0);
            Vector3 b = GetCurrentCellCenter() - new Vector3(5, 0, 0);
            Direction currDir = _vehicle.Route!.CurrentDirection;
            if (currDir != Direction.Left && currDir != Direction.Right)
            {
                a = GetCurrentCellCenter() - new Vector3(0, 0, 5);
                b = GetCurrentCellCenter() - new Vector3(0, 0, -5);
            }
            
            Vector3 prePos = _previousPosition;
            _previousPosition = ReflectPointAcrossLine(_currentPosition, a, b);
            _currentPosition = ReflectPointAcrossLine(prePos, a, b);
            Vector3 pivot1 = _pivotPosition1;
            _pivotPosition1 = ReflectPointAcrossLine(_pivotPosition2, a, b);
            _pivotPosition2 = ReflectPointAcrossLine(pivot1, a, b);
        }


        private void RightTurnSetup()
        {
            float degree = GetTurnRotation();
            
            _previousPosition = _currentPosition;
            
            _pivotPosition1 = _previousPosition + rightTurnPivot1;
            _pivotPosition1 = RotateAroundPivot(_pivotPosition1, _previousPosition, degree);
            
            _pivotPosition2 = GetCurrentCellCenter() + GetSideWaysOffset() + GetStraightOffset() + rightTurnPivot2;
            
            _currentPosition = GetCurrentCellCenter() + GetSideWaysOffset() + GetStraightOffset()
                               + rightTurnDestinationOffset;
            _currentPosition = RotateAroundPivot(_currentPosition, _pivotPosition2, degree);

        }
        
        private void LeftTurnSetup()
        {
            float degree = GetTurnRotation();
            
            _previousPosition = _currentPosition;
            
            _pivotPosition1 = _previousPosition + leftTurnPivot1;
            _pivotPosition1 = RotateAroundPivot(_pivotPosition1, _previousPosition, degree);
            
            _currentPosition = GetCurrentCellCenter() + GetSideWaysOffset() + GetStraightOffset();
            
            _pivotPosition2 = GetCurrentCellCenter() + GetSideWaysOffset() + GetStraightOffset() + leftTurnPivot2;
            _pivotPosition2 = RotateAroundPivot(_pivotPosition2, _currentPosition, degree);
        }

        private float GetTurnRotation()
        {
            if (_vehicle?.Route == null) return 0;
            Direction currentDir = Direction.Right;

            int i = 0;
            while (currentDir != _vehicle.Route.CurrentDirection)
            {
                i++;
                currentDir = currentDir.TurnRightClockwise();
            }

            return i * 90;
        }

        private void WorldPositionsStraight()
        {
            _previousPosition = _currentPosition;
            _currentPosition = GetCurrentCellCenter() + GetSideWaysOffset() + GetStraightOffset();
        }

        private Vector3 GetCurrentCellCenter(Location location = null)
        {
            if (location == null) { location = _vehicle.CurrentLocation; }
            return new Vector3(location!.X, 0, location.Y) * _vehicle.Grid.CellSize
                   + _cellCenter;
        }
        

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

        private Vector3 GetSideWaysOffset()
        {
            if (_vehicle.Route == null) return Vector3.zero;
            
            Direction dir = _vehicle.Route.CurrentDirection;
            if (dir == Direction.Up) return new Vector3(sideWaysOffset,0,0);
            if (dir == Direction.Down) return new Vector3(-sideWaysOffset,0,0);
            if (dir == Direction.Left) return new Vector3(0,0,sideWaysOffset);
            if (dir == Direction.Right) return new Vector3(0,0,-sideWaysOffset);
            return Vector3.zero;
        }
        
        private Vector3 GetStraightOffset(Direction? dir = null)
        {
            if (dir == null)
            {
                if (_vehicle.Route == null) return Vector3.zero;
                dir = _vehicle.Route.CurrentDirection;
            }
            if (dir == Direction.Up) return new Vector3(0,0,straightOffset);
            if (dir == Direction.Down) return new Vector3(0,0,-straightOffset);
            if (dir == Direction.Left) return new Vector3(-straightOffset,0,0);
            if (dir == Direction.Right) return new Vector3(straightOffset,0,0);
            return Vector3.zero;
        }
        
        private static Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, float angleDegrees)
        {
            Quaternion rotation = Quaternion.AngleAxis(angleDegrees, Vector3.up);
            return pivot + rotation * (point - pivot);
        }
        
        public static Vector3 ReflectPointAcrossLine(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector3 dir = (b - a).normalized;
            Vector3 toPoint = point - a;

            Vector3 parallel = Vector3.Dot(toPoint, dir) * dir;
            Vector3 perpendicular = toPoint - parallel;

            return a + parallel - perpendicular;
        }

        private void OnDrawGizmos()
        {
            if (!debugMode) return;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_previousPosition, 0.3f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_pivotPosition1, 0.3f);

            Gizmos.DrawSphere(_pivotPosition2, 0.3f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentPosition, 0.3f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(_previousPosition, _pivotPosition1);
            Gizmos.DrawLine(_pivotPosition1, _pivotPosition2);
            Gizmos.DrawLine(_pivotPosition2, _currentPosition);
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