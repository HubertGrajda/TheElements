using _Scripts.Player;
using Cinemachine;
using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public abstract class CameraBehaviourInitializer : MonoBehaviour
    {
        private PlayerCameraController _playerCameraController;
        protected CinemachineFreeLook VirtualCamera { get; private set; }

        protected virtual void Awake()
        {
            VirtualCamera = GetComponent<CinemachineFreeLook>();
            if (PlayerManager.Instance.TryGetPlayerComponent(out _playerCameraController))
            {
                VirtualCamera.Follow = _playerCameraController.transform;
                VirtualCamera.LookAt = _playerCameraController.CameraLookAtPoint;
            }
        }
    }
}