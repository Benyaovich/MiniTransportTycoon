using System;
using JetBrains.Annotations;
using Model;
using Scene;
using UnityEngine;
using UnityEngine.UIElements;
using UserInterface;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    [SerializeField] public UIDocument uiDocument;
    [SerializeField] private VehicleOwnedListUI vehicleOwnedListUI;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Camera minimapCamera;
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

    private Label _money;

    private VisualElement _gameOverMenuPanel;
    private Button _gameOverMainMenuBtn;
    private Button _saveGameBtn;

    private VisualElement _minimap;

    private float _previousGameSpeedMultiplier;
    
    private bool _isMouseDownOnMinimap;
    private LayerMask _floorLayer;
    private void Awake()
    {
        Instance = this;
        _floorLayer = LayerMask.GetMask("Default");
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

        _money = root.Q<Label>("Money");

        _gameOverMenuPanel = root.Q<VisualElement>("GameOverMenu");
        _gameOverMainMenuBtn = root.Q<Button>("GameOverMainMenuBtn");
        _saveGameBtn = root.Q<Button>("SaveBtn");

        _minimap = root.Q<VisualElement>("Minimap");
        

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
        
        PlayerState.Instance.OnMoneyChanged += SetMoneyLabelText;

        PlayerState.Instance.OnGameOver += PlayerStateOnGameOver;
        _gameOverMainMenuBtn.clicked += MainMenuBtnOnClicked;
        _saveGameBtn.clicked += SaveGameBtnOnclicked;
        
        _minimap.RegisterCallback<MouseDownEvent>(HandleMinimapMouseDown);
        _minimap.RegisterCallback<MouseUpEvent>(HandleMinimapMouseUp);
        _minimap.RegisterCallback<MouseMoveEvent>(HandleMinimapMouseMove);
        _minimap.RegisterCallback<MouseLeaveEvent>(HandleMinimapMouseLeave);
        
        _vehiclePurchasePanel.Disable();
        _vehicleOwnedPanel.Disable();
        _menuPanel.Disable();
        SetMoneyLabelText(PlayerState.Instance, PlayerState.Instance.Money);
        _gameOverMenuPanel.Disable();
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
        
        PlayerState.Instance.OnMoneyChanged -= SetMoneyLabelText;
        
        PlayerState.Instance.OnGameOver -= PlayerStateOnGameOver;
        _gameOverMainMenuBtn.clicked -= MainMenuBtnOnClicked;
        _saveGameBtn.clicked -= SaveGameBtnOnclicked;
        
        _minimap.UnregisterCallback<MouseDownEvent>(HandleMinimapMouseDown);
        _minimap.UnregisterCallback<MouseUpEvent>(HandleMinimapMouseUp);
        _minimap.UnregisterCallback<MouseMoveEvent>(HandleMinimapMouseMove);
        _minimap.UnregisterCallback<MouseLeaveEvent>(HandleMinimapMouseLeave);
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


    private void SetMoneyLabelText([CanBeNull] object sender, int money)
    {
        _money.text = "Money: " + money + "$";
    }
    
    private void PlayerStateOnGameOver(object sender, EventArgs e)
    {
        GameManager.Instance.SetGameSpeedMultiplier(0);
        _vehiclePurchasePanel.Disable();
        _vehicleOwnedPanel.Disable();
        BuildSelectionManager.Instance.ClearSelectedObjectType();
        _gameOverMenuPanel.Enable();
    }

    private void SaveGameBtnOnclicked()
    {
        PersistenceManager.Instance.OnClickSave();
    }

    private void HandleMinimapMouseDown(MouseDownEvent evt)
    {
        _isMouseDownOnMinimap = true;
        MoveCameraToPosition(evt.mousePosition);
    }
    

    private void HandleMinimapMouseMove(MouseMoveEvent evt)
    {
        if (!_isMouseDownOnMinimap) return;
        MoveCameraToPosition(evt.mousePosition);
    }

    private void HandleMinimapMouseUp(MouseUpEvent evt) => _isMouseDownOnMinimap = false;
    private void HandleMinimapMouseLeave(MouseLeaveEvent evt) => _isMouseDownOnMinimap = false;
    
    private void MoveCameraToPosition(Vector2 mousePosition)
    {
        float widthMultiplier = (minimapCamera.scaledPixelWidth / _minimap.layout.width);
        float heightMultiplier = (minimapCamera.scaledPixelHeight / _minimap.layout.width);

        Vector2 convertedMousePosition = new(
            x: (_minimap.layout.width - (Screen.width - mousePosition.x)) * widthMultiplier,
            y: (_minimap.layout.height - mousePosition.y) * heightMultiplier
        );

        Ray cameraRay = minimapCamera.ScreenPointToRay(convertedMousePosition);

        if (Physics.Raycast(cameraRay, out RaycastHit hit, maxDistance: float.MaxValue, _floorLayer))
        {
            cameraController.SetPosition(hit.point);
        }
        
    }

}
