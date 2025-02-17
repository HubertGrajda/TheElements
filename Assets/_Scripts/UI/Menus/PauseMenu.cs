using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PauseMenu : Menu
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button mainMenuBtn;

        [SerializeField] private Menu settingsMenu;

        private ScenesManager _scenesManager;

        protected override void Start()
        {
            base.Start();
        
            _scenesManager = ScenesManager.Instance;
        }

        protected override void InitUIElements()
        {
            restartBtn.onClick.AddListener(OnRestartButtonClicked);
            settingsBtn.onClick.AddListener(settingsMenu.Open);
            mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            UIManager.HideCurrentView();
            _scenesManager.LoadMainMenu();
        }
    
        private void OnRestartButtonClicked()
        {
            UIManager.HideCurrentView();
            _scenesManager.RestartScene();
        }
    }
}
