using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadCellVisual : MonoBehaviour
{
    private RoadCell _roadCell;
    [SerializeField] private GameObject highlightVisual;

    public void Setup(RoadCell roadCell)
    {
        _roadCell = roadCell;
        _roadCell.OnHighlightEnabled += RoadCellOnHighlightEnabled;
        _roadCell.OnHighlightDisabled += RoadCellOnHighlightDisabled;
    }

    private void OnDisable()
    {
        _roadCell.OnHighlightEnabled -= RoadCellOnHighlightEnabled;
        _roadCell.OnHighlightDisabled -= RoadCellOnHighlightDisabled;
    }

    private void RoadCellOnHighlightDisabled(object sender, Location e)
    {
        highlightVisual.SetActive(false);
    }

    private void RoadCellOnHighlightEnabled(object sender, Location e)
    {
        highlightVisual.SetActive(true);
    }
}
