using System;
using Controller.Interfaces;
using Model;
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
        [SerializeField] private BusStopInfoUI busStopInfoUI;
        [SerializeField] private VehicleInfoUI vehicleInfoUI;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            HideAll();
            PlayerState.Instance.OnGameOver += PlayerStateOnGameOver;
        }

        private void OnDisable()
        {
            PlayerState.Instance.OnGameOver -= PlayerStateOnGameOver;
        }

        private void PlayerStateOnGameOver(object sender, EventArgs e)
        {
            HideAll();
        }

        public void ShowFor(IViewable viewable)
        {
            HideAll();

            if (viewable is FacilityVisual factoryView)
            {
                facilityInfoUI.Show(factoryView.Facility);
            }
            else if (viewable is BusStopVisual busStopView)
            {
                busStopInfoUI.Show(busStopView.BusStop);
            }
            else if (viewable is VehicleVisual vehicleView)
            {
                vehicleInfoUI.Show(vehicleView.Vehicle);
            }
        }

        public void HideAll()
        {
            facilityInfoUI.Hide();
            busStopInfoUI.Hide();
            vehicleInfoUI.Hide();
        }
    }
}