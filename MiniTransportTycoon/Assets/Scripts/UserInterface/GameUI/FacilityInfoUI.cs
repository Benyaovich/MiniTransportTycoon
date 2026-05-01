using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.GameUI
{
    public class FacilityInfoUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private Facility _facility;
        
        private VisualElement _panel;
        private Label _producedResource;
        private Label _requiredResource;
        private Label _resourceAmount;
        private Label _currentlyProduces;
        private Label _efficiencyPercentage;
        private Label _requiredAmount;
        private Button _closeBtn;
        
        private void Awake()
        {
            var root = uiDocument.rootVisualElement;

            _panel = root.Q<VisualElement>("FacilityInfoPanel");
            _producedResource = root.Q<Label>("ProducedResource");
            _requiredResource = root.Q<Label>("RequiredResource");
            _resourceAmount = root.Q<Label>("ResourceAmount");
            _currentlyProduces = root.Q<Label>("CurrentlyProduces");
            _efficiencyPercentage = root.Q<Label>("EfficiencyPercentage");
            _requiredAmount = root.Q<Label>("RequiredAmount");
            
            _closeBtn = root.Q<Button>("CloseBtn");
            _closeBtn.clicked += Hide;

            _panel.Disable();
            _requiredResource.Disable();
            _requiredAmount.Disable();
        }

        private void OnDisable()
        {
            _closeBtn.clicked -= Hide;
        }

        private void Update()
        {
            if (_facility == null) return;
            if (!_panel.IsEnabled()) return;

            Refresh(_facility);
        }

        public void Show(Facility facility)
        {
            _facility = facility;
            Refresh(facility);
            _panel.Enable();
        }

        public void Refresh(Facility facility)
        {
            _producedResource.text = $"Produced resource: {facility.ProducedResource}";
            _resourceAmount.text = $"Produces resource amount: {facility.ResourceAmount}/{facility.MaxCapacity}";
            if (facility is ProcessingBuilding processingBuilding)
            {
                _requiredResource.text = $"Required resource: {processingBuilding.RequiredResource}";
                _requiredAmount.text = $"Required resource amount: {processingBuilding.RequiredResourceAmount}/{processingBuilding.RequiredResourceCapacity}";
                _requiredResource.Enable();
                _requiredAmount.Enable();
            }
            else
            {
                _requiredResource.Disable();
                _requiredAmount.Disable();
            }
            _currentlyProduces.text = $"Currently produces between:" +
                                      $" {facility.Rch.CurrentRate - facility.Rch.RateChange}" +
                                      $" and" +
                                      $" {facility.Rch.CurrentRate + facility.Rch.RateChange}";
            _efficiencyPercentage.text = $"Efficiency: {Math.Round(1.0f * facility.Rch.CurrentRate / facility.Rch.MaxRate * 100, 1)}%";
        }

        public void Hide()
        {
            _facility = null;
            _panel.Disable();
        }
    }
}
