using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject aimingCamera;

    private void Start()
    {
        Managers.InputManager.Inputs.PlayerActions.Aim.started += ToggleCrosshair;
        Managers.InputManager.Inputs.PlayerActions.Aim.canceled += ToggleCrosshair;
    }

    private void ToggleCrosshair(InputAction.CallbackContext context)
    {
        var show = context is { started: true, canceled: false };
        
        aimingCamera.SetActive(show);
        crosshair.SetActive(show);
    }

    private void OnDestroy()
    {
        Managers.InputManager.Inputs.PlayerActions.Aim.started -= ToggleCrosshair;
        Managers.InputManager.Inputs.PlayerActions.Aim.canceled -= ToggleCrosshair;
    }
}
