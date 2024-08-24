using _Scripts.Managers;
using UnityEngine;

public class PauseView : View
{
    [SerializeField] private Menu defaultMenu;

    private void Start()
    {
        Managers.GameManager.ChangeState(GameManager.GameState.DuringGameplay); // TODO
        Managers.InputManager.Inputs.UIActions.PauseView.started += ToggleByInput;
    }

    protected override void Show()
    {
        defaultMenu.Open();
        
        base.Show();
    }

    private void OnDestroy()
    {
        Managers.InputManager.Inputs.UIActions.PauseView.started -= ToggleByInput;
    }
}
