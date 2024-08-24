using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class View : MonoBehaviour
{
    [SerializeField] private GameObject containerUI;
    [SerializeField] private bool showCursor;
    [SerializeField] private bool pauseGame;
    
    public bool ShowCursor => showCursor;
    private bool IsShown { get; set; }

    protected virtual void ToggleByInput(InputAction.CallbackContext context)
    {
        if (IsShown)
        {
            Hide();
            return;
        }
        
        Show();
    }
    
    protected virtual void Show()
    {
        Managers.InputManager.Inputs.PlayerActions.Disable();
        Managers.UIManager.CurrentView = this;
        
        if (pauseGame)
        {
            Managers.GameManager.PauseGame();
        }
        
        containerUI.SetActive(true);
        IsShown = true;
        Managers.UIManager.onViewOpened?.Invoke(this);
    }

    public virtual void Hide()
    {
        Managers.UIManager.CurrentView = null;
        Managers.InputManager.Inputs.PlayerActions.Enable();
        Managers.GameManager.ResumeGame();
        
        
        containerUI.SetActive(false);
        IsShown = false;
        Managers.UIManager.onViewClosed?.Invoke(this);
    }
}
