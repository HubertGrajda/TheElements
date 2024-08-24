using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : Menu
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button mainMenuBtn;

    protected override void InitUIElements()
    {
        restartBtn.onClick.AddListener(OnRestartButtonClicked);
        mainMenuBtn.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        Managers.ScenesManager.LoadMainMenu();
    }
    
    private void OnRestartButtonClicked()
    {
        Managers.ScenesManager.RestartScene();
    }
}
