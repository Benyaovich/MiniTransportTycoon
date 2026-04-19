using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.GameUI
{
    public class BusStopInfoUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        
        private VisualElement _panel;
        private Label _resourceType;
        private Label _resourceAmount;
        private Button _closeBtn;
        
        private void Awake()
        {
            var root = uiDocument.rootVisualElement;

            _panel = root.Q<VisualElement>("BusStopInfoPanel");
            _resourceType = root.Q<Label>("ResourceType");
            _resourceAmount = root.Q<Label>("ResourceAmount");
            _closeBtn = root.Q<Button>("CloseBtn");
            
            _closeBtn.clicked += Hide;

            _panel.Disable();
        }

        private void OnDisable()
        {
            _closeBtn.clicked -= Hide;
        }

        public void Show(BusStop busStop)
        {
            Refresh(busStop);
            _panel.Enable();
        }

        public void Refresh(BusStop busStop)
        {
            _resourceType.text = "Requires:" + busStop.ProducedResource;
            _resourceAmount.text = $"Amount of people: {busStop.NumOfPeople}/{busStop.MaxNumOfPeople}";
        }

        public void Hide() => _panel.Disable();
    }
}