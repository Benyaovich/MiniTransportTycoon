using Controller.Grid;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingPreviewManager : MonoBehaviour
{
     [SerializeField] private int previewLayerIndex;
    private Transform _previewObject;
    [CanBeNull] private Transform _previewObjectVisual;

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
        
    }

    private void GridManagerOnSelectedObjectChanged(object sender, [CanBeNull] Transform previewObjectVisual)
    {
        _previewObjectVisual = previewObjectVisual;
        if(_previewObject != null) Destroy(_previewObject.gameObject);
        if (_previewObjectVisual == null) return;
        
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


