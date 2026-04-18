using System;
using Scene;
using UnityEditor.Tilemaps;
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
    private VisualElement _menuPanel;
    private Button _resumeBtn;
    private Button _mainMenuBtn;
    private Button _buyVehiclesBtn;
    private Button _ownedVehiclesBtn;
    private Button _selectRoadBtn;
    private Button _selectBusStopBtn;

    private Button _selectedSpeed;
    private Button _speedPauseBtn;
    private Button _speed1Btn;
    private Button _speed2Btn;
    private Button _speed4Btn;
    
    private VisualElement _vehiclePurchasePanel;
    private VisualElement _vehicleOwnedPanel;

    private float _previousGameSpeedMultiplier;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;

        _menuPanel = root.Q<VisualElement>("Menu");
        _menuBtn = root.Q<Button>("MenuBtn");
        _resumeBtn = root.Q<Button>("ResumeBtn");
        _mainMenuBtn = root.Q<Button>("MainMenuBtn");
        
        _buyVehiclesBtn = root.Q<Button>("BuyVehiclesBtn");
        _ownedVehiclesBtn = root.Q<Button>("OwnedVehiclesBtn");
        _vehiclePurchasePanel = root.Q<VisualElement>("VehiclePurchasePanel");
        _vehicleOwnedPanel = root.Q<VisualElement>("VehicleOwnedPanel");
        
        _speedPauseBtn = root.Q<Button>("SpeedPauseBtn");
        _speed1Btn = root.Q<Button>("Speed1Btn");
        _speed2Btn = root.Q<Button>("Speed2Btn");
        _speed4Btn = root.Q<Button>("Speed4Btn");
        _selectedSpeed = _speed1Btn;

        _selectRoadBtn = root.Q<Button>("SelectRoadBtn");
        _selectBusStopBtn = root.Q<Button>("SelectBusStopBtn");
        

        _menuBtn.clicked += ToggleMenu;
        _resumeBtn.clicked += ToggleMenu;
        _mainMenuBtn.clicked += MainMenuBtnOnClicked;
        
        _buyVehiclesBtn.clicked += ToggleBuyVehicleListView;
        _ownedVehiclesBtn.clicked += ToggleOwnedVehicleListView;
        
        _speedPauseBtn.clicked += SpeedPauseBtnOnClicked;
        _speed1Btn.clicked += Speed1BtnOnClicked;
        _speed2Btn.clicked += Speed2BtnOnClicked;
        _speed4Btn.clicked += Speed4BtnOnClicked;
        
        _selectRoadBtn.clicked += BuildSelectionManager.Instance.SelectDynamicRoadObjectTypeSo;
        _selectBusStopBtn.clicked += BuildSelectionManager.Instance.SelectBusStopObjectTypeSo;
        
        _vehiclePurchasePanel.Disable();
        _vehicleOwnedPanel.Disable();
        _menuPanel.Disable();
    }



    private void OnDisable()
    {
        _menuBtn.clicked -= ToggleMenu;
        _resumeBtn.clicked -= ToggleMenu;
        _mainMenuBtn.clicked -= MainMenuBtnOnClicked;
        
        _buyVehiclesBtn.clicked -= ToggleBuyVehicleListView;
        _ownedVehiclesBtn.clicked -= ToggleOwnedVehicleListView;
        
        _speedPauseBtn.clicked -= SpeedPauseBtnOnClicked;
        _speed1Btn.clicked -= Speed1BtnOnClicked;
        _speed2Btn.clicked -= Speed2BtnOnClicked;
        _speed4Btn.clicked -= Speed4BtnOnClicked;
       
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

    private void ToggleMenu()
    {
        if (_menuPanel.IsEnabled())
        {
            _menuPanel.Disable();
            SetGameSpeed((int)_previousGameSpeedMultiplier);
        }
        else
        {
            _menuPanel.Enable();
            _previousGameSpeedMultiplier = GameManager.Instance.GameSpeedMultiplier;
            SetGameSpeed(0);
        }
    }
    
    
    private async void MainMenuBtnOnClicked()
    {
        await SceneLoader.LoadSceneWithLoadingScreen("MainMenu", "LoadingScene");
    }


    private void SpeedPauseBtnOnClicked() => SetGameSpeed(0);
    private void Speed1BtnOnClicked() => SetGameSpeed(1);
    private void Speed2BtnOnClicked() => SetGameSpeed(2);
    private void Speed4BtnOnClicked() => SetGameSpeed(4);

    private void SetGameSpeed(int multiplier)
    {
        _selectedSpeed.RemoveFromClassList("selectedBtn");
        
        GameManager.Instance.SetGameSpeedMultiplier(multiplier);
        
        if (multiplier == 0){ _selectedSpeed = _speedPauseBtn; }
        else if (multiplier == 1){ _selectedSpeed = _speed1Btn; }
        else if (multiplier == 2){ _selectedSpeed = _speed2Btn; }
        else if (multiplier == 4){ _selectedSpeed = _speed4Btn; }
        
        _selectedSpeed.AddToClassList("selectedBtn");
    }
    
    
    
}
