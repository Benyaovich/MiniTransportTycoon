using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.GameUI
{
    public class VehicleInfoUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        
        private VisualElement _panel;
        private Label _resourceType;
        private Label _resourceAmount;
        private Label _maintenanceCost;
        private Label _moveSpeed;
        private Button _closeBtn;
        
        private void Awake()
        {
            var root = uiDocument.rootVisualElement;

            _panel = root.Q<VisualElement>("VehicleInfoPanel");
            _resourceType = root.Q<Label>("ResourceType");
            _resourceAmount = root.Q<Label>("ResourceAmount");
            _maintenanceCost = root.Q<Label>("MaintenanceCost");
            _moveSpeed = root.Q<Label>("MoveSpeed");
            _closeBtn = root.Q<Button>("CloseBtn");
            
            _closeBtn.clicked += Hide;

            _panel.Disable();
        }

        private void OnDisable()
        {
            _closeBtn.clicked -= Hide;
        }

        public void Show(Vehicle vehicle)
        {
            Refresh(vehicle);
            _panel.Enable();
        }

        public void Refresh(Vehicle vehicle)
        {
            _resourceType.text = "Resource type:" + vehicle.Resource;
            _resourceAmount.text = $"Resource amount: {vehicle.ResourceAmount}/{vehicle.MaxCapacity}";
            _maintenanceCost.text = $"Maintenance cost: {vehicle.MaintenanceCost}";
            _moveSpeed.text = $"Move speed: {vehicle.MoveSpeed}";
        }

        public void Hide() => _panel.Disable();
    }
}