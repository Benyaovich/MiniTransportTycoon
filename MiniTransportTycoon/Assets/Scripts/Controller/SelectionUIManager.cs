using System;
using Controller.Interfaces;
using Model;
using UnityEngine;
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
            viewable.ShowSelectionUI();
        }

        public void ShowFacility(Facility facility) => facilityInfoUI.Show(facility);

        public void ShowBusStop(BusStop busStop) => busStopInfoUI.Show(busStop);

        public void ShowVehicle(Vehicle vehicle) => vehicleInfoUI.Show(vehicle);

        public void HideAll()
        {
            facilityInfoUI.Hide();
            busStopInfoUI.Hide();
            vehicleInfoUI.Hide();
        }
    }
}
