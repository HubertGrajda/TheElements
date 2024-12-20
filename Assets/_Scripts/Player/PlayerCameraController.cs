using _Scripts.Managers;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraLookAtPoint { get; private set; }
    
        private CinemachineFreeLook _aimingCamera;
        private InputAction _inputAction;
    
        private void Start()
        {
            _inputAction = InputsManager.Instance.PlayerActions.Aim;
            _aimingCamera = CameraManager.Instance.AimingFreeLook;

            if (_aimingCamera != null)
            {
                AddListeners();
            }
        }

        private void ToggleCrosshair(InputAction.CallbackContext context)
        {
            var show = context is { started: true, canceled: false };
            _aimingCamera.gameObject.SetActive(show);
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
}