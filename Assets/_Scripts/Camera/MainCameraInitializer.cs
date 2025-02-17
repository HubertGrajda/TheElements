using Cinemachine;
using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class MainCameraInitializer : CameraBehaviourInitializer
    {
        protected override void Awake()
        {
            base.Awake();
            CameraManager.Instance.SetMainBehaviour(VirtualCamera);
        }
    }
}