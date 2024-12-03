using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [field: SerializeField] public Transform CameraLookAtPoint { get; private set; }
    
    private GameObject _crosshair;
    private GameObject _aimingCamera;
    private InputAction _inputAction;
    
    private void Start()
    {
        _inputAction = InputsManager.Instance.PlayerActions.Aim;
        _aimingCamera = CameraManager.Instance.AimingFreeLook.gameObject;
        _crosshair = UIManager.Instance.CurrentHUD.Crosshair.gameObject;
        
        AddListeners();
    }

    private void ToggleCrosshair(InputAction.CallbackContext context)
    {
        var show = context is { started: true, canceled: false };
        
        _aimingCamera.SetActive(show);
        _crosshair.SetActive(show);
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
        if (_inputAction == null) return;
        
        _inputAction.started -= ToggleCrosshair;
        _inputAction.canceled -= ToggleCrosshair;
    }
}
