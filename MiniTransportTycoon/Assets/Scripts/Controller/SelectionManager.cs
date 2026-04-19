using Controller.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        public IViewable SelectedObject { get; private set; }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleSelection();
            }
        }

        private void HandleSelection()
        {
            if (Utils.IsPointerOverBlockingUI()) return;
            
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            
            hit.collider.TryGetComponent(out IViewable selectable);

            if (selectable == null) return;
            ClearSelection();
            Select(selectable);


        }

        public void Select(IViewable selectable)
        {
            SelectedObject = selectable;
            SelectionUIManager.Instance.ShowFor(selectable);
        }

        public void ClearSelection()
        {
            SelectedObject = null;
            SelectionUIManager.Instance.HideAll();
        }
    }
}