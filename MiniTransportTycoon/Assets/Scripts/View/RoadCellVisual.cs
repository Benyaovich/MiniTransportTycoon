using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadCellVisual : MonoBehaviour
{
    private static readonly int BaseColorPropertyId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

    private RoadCell _roadCell;
    [SerializeField] private GameObject highlightVisual;
    private readonly List<Renderer> _highlightRenderers = new();
    private MaterialPropertyBlock _propertyBlock = null!;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _highlightRenderers.AddRange(highlightVisual.GetComponentsInChildren<Renderer>(true));
    }

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
        ApplyHighlightColor(_roadCell.HighlightColor);
        highlightVisual.SetActive(true);
    }

    private void ApplyHighlightColor(Color color)
    {
        foreach (Renderer highlightRenderer in _highlightRenderers)
        {
            highlightRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(BaseColorPropertyId, color);
            _propertyBlock.SetColor(ColorPropertyId, color);
            highlightRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
