using System;
using Controller.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UserInterface.GameUI;
using View;

namespace Controller
{
    public class SelectionUIManager : MonoBehaviour
    {
        public static SelectionUIManager Instance { get; private set; }

        [SerializeField] private FacilityInfoUI facilityInfoUI;
        // [SerializeField] private BusStopInfoUI busStopInfoUI;
        [SerializeField] private VehicleInfoUI vehicleInfoUI;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            HideAll();
        }

        public void ShowFor(IViewable viewable)
        {
            HideAll();

            if (viewable is FacilityVisual factoryView)
            {
                facilityInfoUI.Show(factoryView.Facility);
                return;
            }
                
            // if (selectable is BusStopView busStopView)
            // {
            //     busStopInfoUI.Show(busStopView.BusStop);
            //     return;
            // }

            if (viewable is VehicleVisual vehicleView)
            {
                vehicleInfoUI.Show(vehicleView.Vehicle);
                return;
            }
        }

        public void HideAll()
        {
            facilityInfoUI.Hide();
            // busStopInfoUI.Hide();
            vehicleInfoUI.Hide();
        }
    }
}