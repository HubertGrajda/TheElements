using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button mainMenuBtn;

    [SerializeField] private Menu settingsMenu;
    protected override void InitUIElements()
    {
        restartBtn.onClick.AddListener(OnRestartButtonClicked);
        settingsBtn.onClick.AddListener(settingsMenu.Open);
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        Managers.UIManager.HideCurrentView();
        Managers.ScenesManager.LoadMainMenu();
    }
    
    private void OnRestartButtonClicked()
    {
        Managers.UIManager.HideCurrentView();
        Managers.ScenesManager.RestartScene();
    }
}
