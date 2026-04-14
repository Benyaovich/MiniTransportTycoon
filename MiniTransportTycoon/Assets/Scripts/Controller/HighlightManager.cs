using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controller
{
    public class HighlightManager : MonoBehaviour
    {
        public static HighlightManager Instance { get; private set; }
        public HighlightService HighlightService { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            if(GridManager.Instance == null){ throw new Exception("A GridManager még nem jött létre."); }
            HighlightService = new HighlightService(GridManager.Instance.Grid);
        }

        private void Update()
        {
            List<Timer> timers = HighlightService.Timers.ToList();
            foreach (Timer timer in timers)
            {
                timer.Tick(GameManager.Instance.DeltaTime);
            }
        }
    }
}