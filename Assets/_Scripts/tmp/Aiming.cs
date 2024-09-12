using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject aimingCamera;

    private InputAction _inputAction;
    
    private void Start()
    {
        _inputAction = InputManager.Instance.PlayerActions.Aim;
        
        AddListeners();
    }

    private void ToggleCrosshair(InputAction.CallbackContext context)
    {
        var show = context is { started: true, canceled: false };
        
        aimingCamera.SetActive(show);
        crosshair.SetActive(show);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void AddListeners()
    {
        _inputAction.started += ToggleCrosshair;
        _inputAction.canceled += ToggleCrosshair;
    }

    private void RemoveListeners()
    {
        _inputAction.started -= ToggleCrosshair;
        _inputAction.canceled -= ToggleCrosshair;
    }
}
