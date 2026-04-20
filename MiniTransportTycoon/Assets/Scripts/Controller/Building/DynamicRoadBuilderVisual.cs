using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controller.Building
{
    public class DynamicRoadBuilderVisual : MonoBehaviour
    {
        [SerializeField] private DynamicRoadBuilder dynamicRoadBuilder;
        [SerializeField] private Transform highLightCubePrefab;

        private readonly List<Transform> _highlightCubes = new();
        private void OnEnable()
        {
            dynamicRoadBuilder.OnEndPointChanged += DynamicRoadBuilderOnEndPointChanged;
            dynamicRoadBuilder.OnReset += DynamicRoadBuilderOnReset;
        }
        

        private void OnDisable()
        {
            dynamicRoadBuilder.OnEndPointChanged -= DynamicRoadBuilderOnEndPointChanged;
            dynamicRoadBuilder.OnReset -= DynamicRoadBuilderOnReset;
        }

        private void DynamicRoadBuilderOnEndPointChanged(object sender, List<Location> locations)
        {
            DestroyHighlightCubes();
            foreach (Location location in locations)
            {
                _highlightCubes.Add(Instantiate(highLightCubePrefab, 
                    dynamicRoadBuilder.Grid.GetWorldPosition(location).UVXZ3(),
                    Quaternion.identity, transform));
            }
        }
        
        private void DynamicRoadBuilderOnReset(object sender, EventArgs e)
        {
            DestroyHighlightCubes();
        }

        private void DestroyHighlightCubes()
        {
            foreach (Transform highlightCube in _highlightCubes)
            {
                Destroy(highlightCube.gameObject);
            }
            _highlightCubes.Clear();
        }
    }
}