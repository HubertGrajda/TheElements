using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class DeathMenu : Menu
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button mainMenuBtn;

        private ScenesManager _scenesManager;

        protected override void Start()
        {
            base.Start();
            _scenesManager = ScenesManager.Instance;
        }

        protected override void InitUIElements()
        {
            restartBtn.onClick.AddListener(OnRestartButtonClicked);
            mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            _scenesManager.LoadMainMenu();
        }
    
        private void OnRestartButtonClicked()
        {
            _scenesManager.RestartScene();
        }
    }
}