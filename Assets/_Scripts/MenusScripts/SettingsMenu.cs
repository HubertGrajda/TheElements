using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : Menu
{
    [SerializeField] private Button graphicsMenuBtn;
    [SerializeField] private Button musicMenuBtn;
    [SerializeField] private Button backBtn;

    [SerializeField] private Menu musicMenu;
    [SerializeField] private Menu graphicsMenu;
    
    protected override void InitUIElements()
    {
        graphicsMenuBtn.onClick.AddListener(graphicsMenu.Open);
        musicMenuBtn.onClick.AddListener(musicMenu.Open);
        backBtn.onClick.AddListener(OnBackButtonClicked);
    }
}
