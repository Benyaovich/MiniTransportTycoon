using Controller.Grid;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreviewManager : MonoBehaviour
{
    [SerializeField] private int previewLayerIndex;
    [SerializeField] private Color validPreviewColor = new(0.2f, 0.55f, 1f, 0.5f);
    [SerializeField] private Color invalidPreviewColor = new(1f, 0.2f, 0.2f, 0.5f);

    private static readonly int BaseColorPropertyId = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

    private Transform _previewObject;
    [CanBeNull] private Transform _previewObjectVisual;
    private readonly List<Renderer> _previewRenderers = new();
    private MaterialPropertyBlock _propertyBlock = null!;
    private bool? _lastCanBuildState;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    #region OnEnable - OnDisable

        private void OnEnable()
        {
            GridManager.Instance!.OnSelectedObjectChanged += GridManagerOnSelectedObjectChanged;
        }

        private void OnDisable()
        {
            GridManager.Instance!.OnSelectedObjectChanged -= GridManagerOnSelectedObjectChanged;
        }
    

    #endregion

    private void LateUpdate()
    {
        Vector3 mousePosSnapped = GridManager.Instance!.GetMousePosSnappedToGrid();
        int rotationDegrees = BuildSelectionManager.Instance.CurrentRotationDegrees;
        Vector3 adjustedPreviewPosition = Utils.GetRotatedPlacementPosition(
            mousePosSnapped,
            BuildSelectionManager.Instance.CurrentSelectionSize,
            GridManager.Instance.Grid.CellSize,
            rotationDegrees);

        adjustedPreviewPosition.y = 0.2f;
        transform.position = Vector3.Lerp(transform.position, adjustedPreviewPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Euler(0f, rotationDegrees, 0f);

        UpdatePreviewColor();
    }

    private void GridManagerOnSelectedObjectChanged(object sender, [CanBeNull] Transform previewObjectVisual)
    {
        _previewObjectVisual = previewObjectVisual;
        if(_previewObject != null) Destroy(_previewObject.gameObject);
        _previewRenderers.Clear();
        _lastCanBuildState = null;
        if (_previewObjectVisual == null) return;
        
        CreatePreviewObject();
    }

    private void CreatePreviewObject()
    {
        var t = transform;
        _previewObject = Instantiate(_previewObjectVisual, t.position, t.rotation,
            t);
        SetPreviewLayerForChildren(_previewObject.gameObject);
        CachePreviewRenderers();
        ApplyPreviewColor(validPreviewColor);
    }
    
    private void SetPreviewLayerForChildren(GameObject preview)
    {
        preview.layer = previewLayerIndex;
        for (int i = 0; i < preview.transform.childCount; i++)
        {
            SetPreviewLayerForChildren(preview.transform.GetChild(i).gameObject);
        }
    }

    private void CachePreviewRenderers()
    {
        _previewRenderers.Clear();
        _previewRenderers.AddRange(_previewObject.GetComponentsInChildren<Renderer>(true));
    }

    private void UpdatePreviewColor()
    {
        if (_previewObject == null) return;
        if (!GridManager.Instance!.TryGetMouseGridLocation(out Location location))
        {
            SetPreviewBuildState(false);
            return;
        }

        SetPreviewBuildState(GridManager.Instance.CanBuildSelectedObjectAt(location));
    }

    private void SetPreviewBuildState(bool canBuild)
    {
        if (_lastCanBuildState == canBuild) return;

        _lastCanBuildState = canBuild;
        ApplyPreviewColor(canBuild ? validPreviewColor : invalidPreviewColor);
    }

    private void ApplyPreviewColor(Color color)
    {
        foreach (Renderer renderer in _previewRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(BaseColorPropertyId, color);
            _propertyBlock.SetColor(ColorPropertyId, color);
            renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}


