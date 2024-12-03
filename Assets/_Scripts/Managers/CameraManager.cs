using Cinemachine;
using UnityEngine;

namespace _Scripts.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        private float _xAxisSpeed;
        private float _yAxisSpeed;
        
        private Camera _cameraMain;
        
        public Camera CameraMain => _cameraMain != null ? _cameraMain : _cameraMain = Camera.main;
        public Camera CameraUI { get; private set; }
        private CinemachineFreeLook MainFreeLook { get; set; }
        public CinemachineFreeLook AimingFreeLook { get; private set; }
    
        public void SetUICamera(Camera cam) => CameraUI = cam;
        public void SetMainCamera(Camera cam) => _cameraMain = cam;
    
        public void SetAimBehaviour(CinemachineFreeLook cam) => AimingFreeLook = cam;
        public void SetMainBehaviour(CinemachineFreeLook cam) => MainFreeLook = cam;

        public void ToggleMainCameraMovement(bool enable)
        {
            if (MainFreeLook == null) return;
        
            if (!enable)
            {
                _xAxisSpeed = MainFreeLook.m_XAxis.m_MaxSpeed;
                _yAxisSpeed = MainFreeLook.m_YAxis.m_MaxSpeed;
            }

            MainFreeLook.m_XAxis.m_MaxSpeed = enable ? _xAxisSpeed : 0f;
            MainFreeLook.m_YAxis.m_MaxSpeed = enable ? _yAxisSpeed : 0f;
        }
    }
}