using System;
using UnityEngine;

namespace View
{
    public class VehicleVisual : MonoBehaviour
    {
        private Vehicle _vehicle;
        
        public void Setup(Vehicle vehicle)
        {
            _vehicle = vehicle;
            _vehicle.OnMove += VehicleOnMove;
        }

        private void OnDisable()
        {
            _vehicle.OnMove -= VehicleOnMove;
        }
        
        private void VehicleOnMove(object sender, Vehicle vehicle)
        {
            if (_vehicle.Route == null) return;
            transform.position = 
                new Vector3(_vehicle!.CurrentLocation!.X, 0, _vehicle.CurrentLocation.Y)*_vehicle.Grid.CellSize;
            
        }

    }
}