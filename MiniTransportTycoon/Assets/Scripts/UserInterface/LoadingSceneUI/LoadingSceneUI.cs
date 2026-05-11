using Scene;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.LoadingSceneUI
{
    public class LoadingSceneUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private ProgressBar _loadingProgressBar;
        
        private void OnEnable()
        {
            VisualElement root = uiDocument.rootVisualElement;

            _loadingProgressBar = root.Q<ProgressBar>("LoadingProgressBar");
        }

        private void LateUpdate()
        {
            _loadingProgressBar.value = SceneLoader.LoadingProgress;
            _loadingProgressBar.title = $"Loading... {SceneLoader.LoadingProgress:0}%";
        }
    }
}
