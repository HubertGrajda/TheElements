using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class AimCameraInitializer : CameraBehaviourInitializer
    {
        protected override void Awake()
        {
            base.Awake();
            CameraManager.Instance.SetAimBehaviour(VirtualCamera);
            gameObject.SetActive(false);
        }
    }
}