using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.GameUI
{
    public class BusStopInfoUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private BusStop _busStop;
        
        private VisualElement _panel;
        private Label _resourceType;
        private Label _resourceAmount;
        private Label _currentlyProduces;
        private Label _efficiencyPercentage;
        private Button _closeBtn;
        
        private void Awake()
        {
            var root = uiDocument.rootVisualElement;

            _panel = root.Q<VisualElement>("BusStopInfoPanel");
            _resourceType = root.Q<Label>("ResourceType");
            _resourceAmount = root.Q<Label>("ResourceAmount");
            _currentlyProduces = root.Q<Label>("CurrentlyProduces");
            _efficiencyPercentage = root.Q<Label>("EfficiencyPercentage");
            _closeBtn = root.Q<Button>("CloseBtn");
            
            _closeBtn.clicked += Hide;

            _panel.Disable();
        }

        private void OnDisable()
        {
            _closeBtn.clicked -= Hide;
        }

        private void Update()
        {
            if (_busStop == null) return;
            if (!_panel.IsEnabled()) return;
            
            Refresh(_busStop);
        }

        public void Show(BusStop busStop)
        {
            _busStop = busStop;
            Refresh(busStop);
            _panel.Enable();
        }

        public void Refresh(BusStop busStop)
        {
            _panel.style.height = StyleKeyword.Auto;
            _resourceType.text = "Provides and accepts: " + busStop.ProducedResource;
            _resourceAmount.text = $"Amount of people: {busStop.NumOfPeople}/{busStop.MaxNumOfPeople}";
            if (busStop.City != null)
            {
                _efficiencyPercentage.Enable();
                _currentlyProduces.text = $"People arrive between:" +
                                          $" {busStop.City.Rch.CurrentRate - busStop.City.Rch.RateChange}" +
                                          $" and" +
                                          $" {busStop.City.Rch.CurrentRate + busStop.City.Rch.RateChange}";
                _efficiencyPercentage.text = $"Efficiency: {Math.Round(1.0f * busStop.City.Rch.CurrentRate / busStop.City.Rch.MaxRate * 100, 1)}%";
            }
            else
            {
                _efficiencyPercentage.Disable();
                _currentlyProduces.text = "!! Not connected to city. !!";
            }
        }

        public void Hide()
        {
            _busStop = null;
            _panel.Disable();
        }
    }
}
