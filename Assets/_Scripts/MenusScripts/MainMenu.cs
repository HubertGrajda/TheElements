using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : Menu
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button quitBtn;

        [SerializeField] private Menu settingsMenu;

        protected override void Start()
        {
            base.Start();
        
            UIManager.SetCurrentMenu(this);
        }

        protected override void InitUIElements()
        {
            startGameBtn.onClick.AddListener(OnStartGameButtonClicked);
            settingsBtn.onClick.AddListener(settingsMenu.Open);
            quitBtn.onClick.AddListener(Application.Quit);
        }
    
        private void OnStartGameButtonClicked()
        {
            ScenesManager.Instance.NextScene();
        }
    }
}
