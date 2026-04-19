using Controller.Interfaces;
using UnityEngine;

namespace View
{
    public class FacilityVisual : MonoBehaviour, IViewable
    {
        public Facility Facility { get; private set; }

        public void Setup(Facility facility)
        {
            Facility = facility;
        }
    }
}