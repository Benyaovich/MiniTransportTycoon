using System;
using System.Collections.Generic;
using UnityEngine;

public class ForestVisual : MonoBehaviour
{
    private Forest _forest;
    [SerializeField] private List<GameObject> trees = null;

    public void Setup(Forest forest)
    {
        _forest = forest;
        _forest.OnGrow += ForestOnGrow;
    }

    private void OnDisable()
    {
        _forest.OnGrow -= ForestOnGrow;
    }

    private void ForestOnGrow(object sender, Location e)
    {
        trees[_forest.NumOfTrees - 1].SetActive(true);
        if (_forest.NumOfTrees == 4)
        {
            _forest.OnGrow -= ForestOnGrow;
        }
    }
}
