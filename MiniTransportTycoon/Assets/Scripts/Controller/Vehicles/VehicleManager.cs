
#nullable enable
using System;
using System.Collections.Generic;
using Controller.Grid;
using Model;
using Model.Interfaces;
using Model.Vehicles;
using ScriptableObjects.Vehicles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Vehicles
{
    public class VehicleManager : MonoBehaviour
    {
        public static VehicleManager Instance { get; private set; } = null!;
        public event EventHandler? OnVehicleBought;
        public event EventHandler? OnVehicleSold;
        
        [SerializeField] private List<VehicleSO> vehicleSos = new();
        public IVehicleStorage VehicleStorage => _vehicleStorage;
        public List<VehicleSO> VehicleSos => vehicleSos;
        public CityService CityService => _cityService;
        
        private IVehicleStorage _vehicleStorage = null!;
        private VehicleVisualService _vehicleVisualService = null!;
        private CityService _cityService = null!;
        
        private void Awake()
        {
            Instance = this;
            _cityService = GridManager.Instance!.CityService;
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
                advancable.Tick(GameManager.Instance.DeltaTime);
            }
        }

        public void BuyVehicle(VehicleSO vehicleSo)
        {
            Vehicle vehicle = vehicleSo.Create(GridManager.Instance!.Grid);
            vehicle.SetCityService(_cityService);
            _vehicleStorage.AddVehicle(vehicle);
            PlayerState.Instance.SpendMoney(vehicle.Price);
            OnVehicleBought?.Invoke(this, EventArgs.Empty);
        }

        public void SellVehicle(Vehicle vehicle)
        {
            _vehicleStorage.RemoveVehicle(vehicle);
            OnVehicleSold?.Invoke(this, EventArgs.Empty);
        }

        public string GetVehicleDisplayName(Vehicle vehicle)
        {
            VehicleSO? vehicleSo = vehicleSos.Find(x => x.VehicleType == vehicle.GetType());
            return vehicleSo?.displayName ?? vehicle.GetType().Name;
        }

        public string GetVehicleDisplayLabel(Vehicle vehicle)
        {
            return $"{GetVehicleDisplayName(vehicle)} #{vehicle.Identifier}";
        }
    }
}
