using System;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;

    [SerializeField] private Menu settingsMenu;

    protected override void Start()
    {
        base.Start();
        
        Managers.UIManager.SetCurrentMenu(this);
    }

    protected override void InitUIElements()
    {
        startGameBtn.onClick.AddListener(OnStartGameButtonClicked);
        settingsBtn.onClick.AddListener(settingsMenu.Open);
        quitBtn.onClick.AddListener(Application.Quit);
    }
    
    private void OnStartGameButtonClicked()
    {
        Managers.ScenesManager.NextScene();
    }
}
