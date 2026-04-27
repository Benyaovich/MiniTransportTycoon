using Model;
using Scene;
using UnityEngine;
using UnityEngine.UIElements;

namespace UserInterface.MainMenuUI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private Button _newGameBtn;
        private Button _loadGameBtn;
        private Button _exitGameBtn;
        private void OnEnable()
        {
            var root = uiDocument.rootVisualElement;
            _newGameBtn = root.Q<Button>("NewGameBtn");
            _loadGameBtn = root.Q<Button>("LoadGameBtn");
            _exitGameBtn = root.Q<Button>("ExitGameBtn");
            
            _newGameBtn.clicked += NewGameBtnOnClicked;
            _exitGameBtn.clicked += ExitGameBtnOnClicked;
            _loadGameBtn.clicked += LoadGameBtnOnclicked;
        }
        


        private void OnDisable()
        {
            _newGameBtn.clicked -= NewGameBtnOnClicked;
            _exitGameBtn.clicked -= ExitGameBtnOnClicked;
            _loadGameBtn.clicked -= LoadGameBtnOnclicked;
        }

        private async void NewGameBtnOnClicked()
        {
            PlayerState.Instance.ResetPlayerState();
            await SceneLoader.LoadSceneWithLoadingScreen("GameScene", "LoadingScene");
        }
        
        private void ExitGameBtnOnClicked()
        {
            Application.Quit();
        }
        
        private async void LoadGameBtnOnclicked()
        {
            PlayerState.Instance.ResetPlayerState();
            await SceneLoader.LoadSceneWithLoadingScreen("GameScene", "LoadingScene");
            PersistenceManager.Instance.OnClickOpen();
        }
    }
}
