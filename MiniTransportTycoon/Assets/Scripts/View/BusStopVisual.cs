using Controller.Interfaces;
using Controller;
using UnityEngine;

namespace View
{
    public class BusStopVisual : MonoBehaviour, IViewable
    {
        public BusStop BusStop { get; private set; }

        public void Setup(BusStop busStop)
        {
            BusStop = busStop;
        }

        public void ShowSelectionUI()
        {
            SelectionUIManager.Instance.ShowBusStop(BusStop);
        }
    }
}
