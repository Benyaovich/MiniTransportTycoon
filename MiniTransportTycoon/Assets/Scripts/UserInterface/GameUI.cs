using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UserInterface;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    [SerializeField] public UIDocument uiDocument;
    [SerializeField] private VehicleOwnedListUI vehicleOwnedListUI;
    private Button _menuBtn;
    private Button _buyVehiclesBtn;
    private Button _ownedVehiclesBtn;
    private Button _selectRoadBtn;
    private Button _selectBusStopBtn;
    
    private VisualElement _vehiclePurchasePanel;
    private VisualElement _vehicleOwnedPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        _menuBtn = root.Q<Button>("MenuBtn");
        _buyVehiclesBtn = root.Q<Button>("BuyVehiclesBtn");
        _ownedVehiclesBtn = root.Q<Button>("OwnedVehiclesBtn");
        _vehiclePurchasePanel = root.Q<VisualElement>("VehiclePurchasePanel");
        _vehicleOwnedPanel = root.Q<VisualElement>("VehicleOwnedPanel");
        _buyVehiclesBtn.clicked += ToggleBuyVehicleListView;
        _ownedVehiclesBtn.clicked += ToggleOwnedVehicleListView;

        _vehiclePurchasePanel.Disable();
        _vehicleOwnedPanel.Disable();
        
        _selectRoadBtn = root.Q<Button>("SelectRoadBtn");
        _selectRoadBtn.clicked += BuildSelectionManager.Instance.SelectDynamicRoadObjectTypeSo;

        _selectBusStopBtn = root.Q<Button>("SelectBusStopBtn");
        _selectBusStopBtn.clicked += BuildSelectionManager.Instance.SelectBusStopObjectTypeSo;
    }

    private void OnDisable()
    {
        _buyVehiclesBtn.clicked -= ToggleBuyVehicleListView;
        _ownedVehiclesBtn.clicked -= ToggleOwnedVehicleListView;
        _selectRoadBtn.clicked -= BuildSelectionManager.Instance.SelectDynamicRoadObjectTypeSo;
        _selectBusStopBtn.clicked -= BuildSelectionManager.Instance.SelectBusStopObjectTypeSo;
    }

    private void ToggleOwnedVehicleListView()
    {
        _vehiclePurchasePanel.Disable();
        vehicleOwnedListUI.InitialState();
        vehicleOwnedListUI.RefreshList();
        _vehicleOwnedPanel.ToggleVisibility();
    }

    private void ToggleBuyVehicleListView()
    {
        _vehicleOwnedPanel.Disable();
        vehicleOwnedListUI.InitialState();
        _vehiclePurchasePanel.ToggleVisibility();
    }


    
    
    
    
}
