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
            
        }

    }
}