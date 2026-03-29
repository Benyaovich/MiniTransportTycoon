
using System;
using System.Collections.Generic;
using Model.Interfaces;
using Model.Vehicles;
using Model.Vehicles.CargoTrucks;
using ScriptableObjects.Vehicles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Vehicles
{
    public class VehicleManager : MonoBehaviour
    {
        public event EventHandler OnVehicleBought;
        public event EventHandler OnVehicleSold;
        
        [SerializeField] private List<VehicleSO> vehicleSos = new();
        public IVehicleStorage VehicleStorage => _vehicleStorage;
        public List<VehicleSO> VehicleSos => vehicleSos;
        
        private IVehicleStorage _vehicleStorage;
        private VehicleVisualService _vehicleVisualService;
        
        private void Awake()
        {
            _vehicleStorage = new VehicleStorage();
            _vehicleVisualService = new VehicleVisualService(_vehicleStorage, transform, vehicleSos);
        }

        private void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                BuyVehicle(vehicleSos[0]);
            }

            foreach (IAdvancable advancable in _vehicleStorage.Vehicles)
            {
                advancable.Tick(Time.deltaTime);
            }
        }

        public void BuyVehicle(VehicleSO vehicleSo)
        {
            Vehicle vehicle = vehicleSo.Create();
            _vehicleStorage.AddVehicle(vehicle);
            OnVehicleBought?.Invoke(this, EventArgs.Empty);
        }

        public void SellVehicle(Vehicle vehicle)
        {
            _vehicleStorage.RemoveVehicle(vehicle);
            OnVehicleSold?.Invoke(this, EventArgs.Empty);
        }
    }
}
