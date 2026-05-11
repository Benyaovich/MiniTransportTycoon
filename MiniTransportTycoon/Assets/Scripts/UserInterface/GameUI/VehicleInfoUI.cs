#nullable enable
using System;
using Controller.Vehicles;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.GameUI
{
    public class VehicleInfoUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument = null!;
        [SerializeField] private VehicleManager vehicleManager = null!;
        private VehicleOwnedListUI _vehicleOwnedListUI = null!;

        private Vehicle? _vehicle;

        private VisualElement _panel = null!;

        private Label _vehicleName = null!;
        private Label _resourceType = null!;
        private Label _resourceAmount = null!;
        private Label _maintenanceCost = null!;
        private Label _moveSpeed = null!;
        private Label _depositePerTile = null!;

        private Button _closeBtn = null!;
        private Button _routeBtn = null!;
        private Button _sellBtn = null!;

        private void Awake()
        {
            VisualElement root = uiDocument.rootVisualElement;

            _panel = root.Q<VisualElement>("VehicleInfoPanel");

            _vehicleName = root.Q<Label>("VehicleName");
            _resourceType = root.Q<Label>("ResourceType");
            _resourceAmount = root.Q<Label>("ResourceAmount");
            _maintenanceCost = root.Q<Label>("MaintenanceCost");
            _moveSpeed = root.Q<Label>("MoveSpeed");
            _depositePerTile = root.Q<Label>("DepositPerCell");

            _closeBtn = root.Q<Button>("CloseBtn");
            _routeBtn = root.Q<Button>("RouteBtn");
            _sellBtn = root.Q<Button>("SellBtn");

            _closeBtn.clicked += Hide;
            _routeBtn.clicked += StartRouteCreation;
            _sellBtn.clicked += SellVehicle;
            
            _panel.Disable();
        }

        private void OnDestroy()
        {
            _closeBtn.clicked -= Hide;
            _routeBtn.clicked -= StartRouteCreation;
            _sellBtn.clicked -= SellVehicle;
        }

        private void Start()
        {
            _vehicleOwnedListUI = global::GameUI.Instance.VehicleOwnedListUI;
        }

        private void Update()
        {
            if (_vehicle == null)
                return;

            if (_panel.style.display == DisplayStyle.None)
                return;

            Refresh(_vehicle);
        }

        public void Show(Vehicle vehicle)
        {
            _vehicle = vehicle;
            
            if(_vehicle is Bus){ _depositePerTile.Disable(); }
            else{ _depositePerTile.Enable(); }
            Refresh(vehicle);

            _panel.Enable();
        }

        public void Refresh(Vehicle vehicle)
        {
            bool routeCreationActiveForVehicle = _vehicleOwnedListUI.IsRouteCreationActiveFor(vehicle);
            bool routeCreationInProgress = _vehicleOwnedListUI.IsRouteCreationInProgress();

            _vehicleName.text = vehicleManager.GetVehicleDisplayLabel(vehicle);
            _resourceType.text = $"Resource type: {vehicle.Resource}";
            _resourceAmount.text = $"Resource amount: {vehicle.ResourceAmount}/{vehicle.MaxCapacity}";
            _maintenanceCost.text = $"Maintenance cost: {vehicle.MaintenanceCost}";
            _moveSpeed.text = $"Move speed: {vehicle.MoveSpeed}";
            _depositePerTile.text = $"Deposits {vehicle.DepositPerCellInCity} each tile in cities";
            _routeBtn.text = routeCreationActiveForVehicle
                ? "Cancel"
                : vehicle.Route is null ? "Assign Route" : "Edit Route";
            _routeBtn.SetEnabled(!routeCreationInProgress || routeCreationActiveForVehicle);
        }

        public void Hide()
        {
            _vehicle = null;

            _panel.Disable();
        }

        private void StartRouteCreation()
        {
            if (_vehicle == null)
                return;

            _vehicleOwnedListUI.BeginRouteCreationForVehicle(_vehicle);
        }

        private void SellVehicle()
        {
            if (_vehicle == null)
                return;

            _vehicleOwnedListUI.CancelRouteCreation();

            vehicleManager.SellVehicle(_vehicle);

            Hide();
        }
    }
}
