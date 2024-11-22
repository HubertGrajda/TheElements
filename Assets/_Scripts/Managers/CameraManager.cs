using Cinemachine;
using UnityEngine;

namespace _Scripts.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private Camera cameraMain;
        [SerializeField] private Camera cameraUI;
    
        [SerializeField] private CinemachineFreeLook aimingFreeLook;
        [SerializeField] private CinemachineFreeLook mainFreeLook;

        public Camera CameraUI => cameraUI;
        public Camera CameraMain => cameraMain;

        private CinemachineFreeLook MainFreeLook => mainFreeLook;

        private float _xAxisSpeed;
        private float _yAxisSpeed;
    
        //TODO:
        public void SetUICamera(Camera cam) => cameraUI = cam;
        public void SetMainCamera(Camera cam) => cameraMain = cam;
    
        public void SetAimBehaviour(CinemachineFreeLook cam) => aimingFreeLook = cam;
        public void SetMainBehaviour(CinemachineFreeLook cam) => mainFreeLook = cam;

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