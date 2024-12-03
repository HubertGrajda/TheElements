using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

namespace Cameras
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