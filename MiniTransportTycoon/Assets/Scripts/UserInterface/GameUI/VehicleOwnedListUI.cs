#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Vehicles;
using ScriptableObjects.Vehicles;
using UnityEngine;
using UnityEngine.UIElements;
using UserInterface;

public class VehicleOwnedListUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument = null!;
    [SerializeField] private VehicleManager vehicleManager = null!;
    [SerializeField] private VisualTreeAsset listItemTemplate = null!;

    private ScrollView _vehicleList = null!;
    private VisualElement _infoPanel = null!;
    private RouteCreationManager _routeCreationManager = null!;

    private readonly List<Button> _pathCreationButtons = new();
    private readonly Dictionary<Button, Action> _pathButtonHandlers = new();

    private EventHandler<List<Location>>? _routeCreatedHandler;
    private Vehicle? _vehicleInRouteCreation;

    private void Awake()
    {
        _routeCreationManager = RouteCreationManager.Instance;
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;

        _vehicleList = root.Q<ScrollView>("VehicleOwnedList");
        _infoPanel = root.Q<VisualElement>("NoVehiclesOwnedInfoPanel");

        Label? infoText = _infoPanel.Q<Label>("Text");
        if (infoText is not null)
        {
            infoText.text = "You don't have any vehicles.";
        }

        vehicleManager.OnVehicleSold += HandleVehicleChanged;
        vehicleManager.OnVehicleBought += HandleVehicleChanged;

        RefreshList();
    }

    private void OnDisable()
    {
        vehicleManager.OnVehicleSold -= HandleVehicleChanged;
        vehicleManager.OnVehicleBought -= HandleVehicleChanged;

        InitialState();
    }

    public void InitialState()
    {
        CleanupPathButtonHandlers();
        CancelRouteCreation();
    }

    private void HandleVehicleChanged(object? sender, EventArgs e)
    {
        RefreshList();
    }

    public void RefreshList()
    {
        CleanupPathButtonHandlers();

        _vehicleList.Clear();
        _pathCreationButtons.Clear();

        UpdateInfoPanel();

        foreach (Vehicle vehicle in vehicleManager.VehicleStorage.Vehicles)
        {
            CreateVehicleListItem(vehicle);
        }
    }

    private void CreateVehicleListItem(Vehicle vehicle)
    {
        VehicleSO vehicleSo = GetVehicleSO(vehicle);

        VisualElement element = listItemTemplate.Instantiate();

        element.Q<Label>("Name").text = vehicleSo.displayName;
        element.Q<Label>("Capacity").text = $"Capacity: {vehicleSo.maxCarryCapacity}";
        element.Q<Label>("Speed").text = $"Speed: {Math.Round(1.0 / vehicleSo.speed * 150):0}";
        element.Q<Label>("Maintenance").text = $"Maintenance: {vehicleSo.maintenanceCost}";

        Button pathBtn = element.Q<Button>("PathBtn");
        Button sellBtn = element.Q<Button>("SellBtn");

        RegisterPathButton(pathBtn, vehicle);

        sellBtn.clicked += () => SellVehicle(vehicle);

        _vehicleList.Add(element);
    }

    private VehicleSO GetVehicleSO(Vehicle vehicle)
    {
        VehicleSO? vehicleSo = vehicleManager.VehicleSos
            .Find(x => x.VehicleType == vehicle.GetType());

        if (vehicleSo is null)
        {
            throw new NullReferenceException($"Nincs VehicleSO ehhez: {vehicle.GetType().Name}");
        }

        return vehicleSo;
    }

    private void RegisterPathButton(Button pathBtn, Vehicle vehicle)
    {
        _pathCreationButtons.Add(pathBtn);

        Action handler = () => OnPathButtonClicked(pathBtn, vehicle);

        _pathButtonHandlers[pathBtn] = handler;
        pathBtn.clicked += handler;
    }

    private void OnPathButtonClicked(Button pathBtn, Vehicle vehicle)
    {
        if (_routeCreationManager.InRouteCreation)
        {
            CancelRouteCreation();
            RefreshList();
            return;
        }

        DisableOtherPathButtons(pathBtn);
        pathBtn.text = "Cancel";

        BeginRouteCreationForVehicle(vehicle);
    }

    public void BeginRouteCreationForVehicle(Vehicle vehicle)
    {
        if (_routeCreationManager.InRouteCreation)
        {
            CancelRouteCreation();
            RefreshList();
            return;
        }

        _vehicleInRouteCreation = vehicle;

        CleanupRouteCreation();

        _routeCreationManager.StartRouteCreation();

        _routeCreatedHandler = (_, route) =>
        {
            CleanupRouteCreation();

            vehicle.SetRoute(new Route(route, _routeCreationManager.PathHandler));

            _vehicleInRouteCreation = null;

            RefreshList();
        };

        _routeCreationManager.OnRouteCreated += _routeCreatedHandler;
    }

    public void CancelRouteCreation()
    {
        CleanupRouteCreation();

        _vehicleInRouteCreation = null;

        if (_routeCreationManager.InRouteCreation)
        {
            _routeCreationManager.ExitRouteCreation();
        }
    }

    private void SellVehicle(Vehicle vehicle)
    {
        if (_vehicleInRouteCreation == vehicle)
        {
            CancelRouteCreation();
        }

        vehicleManager.SellVehicle(vehicle);
    }

    private void DisableOtherPathButtons(Button current)
    {
        foreach (Button button in _pathCreationButtons.Where(x => x != current))
        {
            button.SetEnabled(false);
        }
    }

    private void UpdateInfoPanel()
    {
        bool hasNoVehicles = vehicleManager.VehicleStorage.Vehicles.Count == 0;
        if (hasNoVehicles)
        {
            _infoPanel.Enable();
        }
        else
        {
            _infoPanel.Disable();
        }
    }

    private void CleanupPathButtonHandlers()
    {
        foreach ((Button button, Action handler) in _pathButtonHandlers)
        {
            button.clicked -= handler;
        }

        _pathButtonHandlers.Clear();
    }

    private void CleanupRouteCreation()
    {
        if (_routeCreatedHandler is not null)
        {
            _routeCreationManager.OnRouteCreated -= _routeCreatedHandler;
            _routeCreatedHandler = null;
        }
    }
}