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

    private void Awake()
    {
        _routeCreationManager = RouteCreationManager.Instance;
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;

        _vehicleList = root.Q<ScrollView>("VehicleOwnedList");
        _infoPanel = root.Q<VisualElement>("NoVehiclesOwnedInfoPanel");
        _infoPanel.Q<Label>("Text").text = "You don't have any vehicles.";

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
        CleanupRouteCreation();
        if (_routeCreationManager.InRouteCreation)
        {
            _routeCreationManager.ExitRouteCreation();
        }
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
            VehicleSO? vehicleSo = vehicleManager.VehicleSos.Find(x => x.VehicleType == vehicle.GetType());
            if (vehicleSo is null)
            {
                throw new NullReferenceException($"Nincs VehicleSO ehhez: {vehicle.GetType().Name}");
            }

            VisualElement element = listItemTemplate.Instantiate();

            element.Q<Label>("Name").text = vehicleSo.displayName;
            element.Q<Label>("Capacity").text = $"Capacity: {vehicleSo.maxCarryCapacity}";
            element.Q<Label>("Speed").text = $"Speed: {Math.Round(1.0 / vehicleSo.speed * 150):0}";
            element.Q<Label>("Maintenance").text = $"Maintenance: {vehicleSo.maintenanceCost}";

            Button pathBtn = element.Q<Button>("PathBtn");
            Button sellBtn = element.Q<Button>("SellBtn");

            _pathCreationButtons.Add(pathBtn);

            Action pathHandler = () => OnPathButtonClicked(pathBtn, vehicle);
            _pathButtonHandlers[pathBtn] = pathHandler;
            pathBtn.clicked += pathHandler;

            sellBtn.clicked += SellBtnClicked;

            void SellBtnClicked()
            {
                sellBtn.clicked -= SellBtnClicked;

                if (_pathButtonHandlers.TryGetValue(pathBtn, out Action? storedPathHandler))
                {
                    pathBtn.clicked -= storedPathHandler;
                    _pathButtonHandlers.Remove(pathBtn);
                }

                _pathCreationButtons.Remove(pathBtn);

                if (_routeCreationManager.InRouteCreation)
                {
                    CleanupRouteCreation();
                    _routeCreationManager.ExitRouteCreation();
                }

                vehicleManager.SellVehicle(vehicle);
            }

            
            _vehicleList.Add(element);
        }
    }

    private void OnPathButtonClicked(Button pathBtn, Vehicle vehicle)
    {
        if (_routeCreationManager.InRouteCreation)
        {
            CleanupRouteCreation();
            _routeCreationManager.ExitRouteCreation();
            RefreshList();
            return;
        }

        DisableOtherPathButtons(pathBtn);
        pathBtn.text = "Cancel";

        _routeCreationManager.StartRouteCreation();

        _routeCreatedHandler = OnRouteCreated;
        _routeCreationManager.OnRouteCreated += _routeCreatedHandler;

        void OnRouteCreated(object? sender, List<Location> route)
        {
            CleanupRouteCreation();
            vehicle.SetRoute(new Route(route, _routeCreationManager.PathHandler));
            RefreshList();
        }
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
        if (vehicleManager.VehicleStorage.Vehicles.Count == 0)
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