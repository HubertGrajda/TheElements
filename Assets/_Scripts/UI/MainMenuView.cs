using _Scripts.Managers;
using UnityEngine;

public class MainMenuView : View
{
    [SerializeField] private GameObject pointer;
    private void Start()
    {
        Managers.GameManager.ChangeState(GameManager.GameState.MainMenu);
        Managers.UIManager.CursorController.ChangePointer(pointer);
        Show();
    }
}
