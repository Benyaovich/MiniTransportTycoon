#nullable enable
using System;
using System.Collections.Generic;
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
    

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        _vehicleList = root.Q<ScrollView>("VehicleOwnedList");
        _infoPanel = root.Q<VisualElement>("NoVehiclesOwnedInfoPanel");
        _infoPanel.Q<Label>("Text").text = "You don't have any vehicles.";

        vehicleManager.OnVehicleSold += HandleVehicleSold;
        
        RefreshList();
    }

    private void OnDisable()
    {
        vehicleManager.OnVehicleBought -= HandleVehicleSold;
    }

    private void HandleVehicleSold(object sender, EventArgs e)
    {
        RefreshList();
    }

    public void RefreshList()
    {
        InfoPanel();
        _vehicleList.Clear();
        foreach (Vehicle vehicle in vehicleManager.VehicleStorage.Vehicles)
        {
            VehicleSO? vehicleSo = vehicleManager.VehicleSos.Find(x => x.VehicleType == vehicle.GetType());
            if(vehicleSo is null){ throw new NullReferenceException("Nincs ilyen VehicleSO"); }

            VisualElement element = listItemTemplate.Instantiate();
            element.Q<Label>("Name").text = vehicleSo.displayName;
            element.Q<Label>("Capacity").text = "Capacity: " + vehicleSo.maxCarryCapacity;
            element.Q<Label>("Speed").text = "Speed: " + Math.Round(1.0 / vehicleSo.speed * 150).ToString("0");
            element.Q<Label>("Maintenance").text = "Maintenance: " + vehicleSo.maintenanceCost;
        
        
            Button pathBtn = element.Q<Button>("PathBtn");
            pathBtn.clicked += PathBtnClicked;
            
            void PathBtnClicked()
            {
                RouteCreationManager routeCreationManager = RouteCreationManager.Instance;
                if (routeCreationManager.InRouteCreation)
                {
                    routeCreationManager.ExitRouteCreation();
                    return;
                }
                
                routeCreationManager.StartRouteCreation();
                
                void OnRouteCreated(object? sender, List<Location> route)
                {
                    routeCreationManager.OnRouteCreated -= OnRouteCreated;
                    vehicle.SetRoute(new Route(route));
                }

                routeCreationManager.OnRouteCreated += OnRouteCreated;
            }
            
            
            Button sellBtn = element.Q<Button>("SellBtn");
            sellBtn.clicked += SellBtnClicked;
            
            void SellBtnClicked()
            {
                sellBtn.clicked -= SellBtnClicked;
                pathBtn.clicked -= PathBtnClicked;

                vehicleManager.SellVehicle(vehicle);
            }

            
            _vehicleList.Add(element);
        }
    }


    private void InfoPanel()
    {
        if (vehicleManager.VehicleStorage.Vehicles.Count == 0){ _infoPanel.Enable(); }
        else{ _infoPanel.Disable(); }
    }
}