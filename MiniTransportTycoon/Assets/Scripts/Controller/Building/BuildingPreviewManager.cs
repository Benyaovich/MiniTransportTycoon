using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingPreviewManager : MonoBehaviour
{
     [SerializeField] private int previewLayerIndex;
    private Transform _previewObject;
    private Transform _previewObjectVisual;

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
        mousePosSnapped.y = 0.1f;
        transform.position = Vector3.Lerp(transform.position, mousePosSnapped, Time.deltaTime * 15f);
        
    }

    private void GridManagerOnSelectedObjectChanged(object sender, Transform previewObjectVisual)
    {
        _previewObjectVisual = previewObjectVisual;
        if(_previewObject is not null) Destroy(_previewObject.gameObject);
        if (_previewObjectVisual is null) return;
        CreatePreviewObject();
    }

    private void CreatePreviewObject()
    {
        var t = transform;
        _previewObject = Instantiate(_previewObjectVisual, t.position, t.rotation,
            t);
        SetPreviewLayerForChildren(_previewObject.gameObject);
    }
    
    private void SetPreviewLayerForChildren(GameObject preview)
    {
        preview.layer = previewLayerIndex;
        for (int i = 0; i < preview.transform.childCount; i++)
        {
            SetPreviewLayerForChildren(preview.transform.GetChild(i).gameObject);
        }
    }
}
