using System;
using Controller.Vehicles;
using ScriptableObjects.Vehicles;
using UnityEngine;
using UnityEngine.UIElements;

public class VehiclePurchaseListUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VehicleManager vehicleManager;
    [SerializeField] private VisualTreeAsset listItemTemplate = null!;
    
    private ScrollView _vehicleList;

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        _vehicleList = root.Q<ScrollView>("VehiclePurchaseList");

        RefreshList();
    }

    private void RefreshList()
    {
        foreach (VehicleSO vehicleSo in vehicleManager.VehicleSos)
        {
            VisualElement element = listItemTemplate.Instantiate();
            element.Q<Label>("Name").text = vehicleSo.displayName;
            element.Q<Label>("Capacity").text = "Capacity: " + vehicleSo.maxCarryCapacity;
            element.Q<Label>("Speed").text = "Speed: " + Math.Round(1.0 / vehicleSo.speed * 150).ToString("0");
            element.Q<Label>("Maintenance").text = "Maintenance: " + vehicleSo.maintenanceCost;
            
            
            Button buyBtn = element.Q<Button>("BuyBtn");
            buyBtn.text = vehicleSo.price.ToString();
            buyBtn.clicked += () => vehicleManager.BuyVehicle(vehicleSo);
            
            _vehicleList.Add(element);
        }

    }
}