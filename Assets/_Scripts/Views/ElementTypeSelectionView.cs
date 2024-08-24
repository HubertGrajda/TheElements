using _Scripts.Managers;
using UnityEngine.InputSystem;

public class ElementTypeSelectionView : View
{
    private void Start()
    {
        Managers.InputManager.Inputs.UIActions.ElementTypeSelectionView.started += ToggleByInput;
        Managers.InputManager.Inputs.UIActions.ElementTypeSelectionView.canceled += ToggleByInput;
    }

    private void OnDisable()
    {
        Managers.InputManager.Inputs.UIActions.ElementTypeSelectionView.started -= ToggleByInput;
        Managers.InputManager.Inputs.UIActions.ElementTypeSelectionView.canceled -= ToggleByInput;
    }

    protected override void ToggleByInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Show();
        }

        if (context.canceled)
        {
            StartCoroutine(Managers.GameManager.ChangeTimeScale(1f, 0.5f));
            Hide();
        }
    }

    protected override void Show()
    {
        base.Show();
        Managers.CamerasManager.ToggleMainCameraMovement(false);
        StartCoroutine(Managers.GameManager.ChangeTimeScale(0.1f, 0.5f));
    }

    public override void Hide()
    {
        base.Hide();
        Managers.CamerasManager.ToggleMainCameraMovement(true);
        StartCoroutine(Managers.GameManager.ChangeTimeScale(1f, 0.5f));
    }
}
