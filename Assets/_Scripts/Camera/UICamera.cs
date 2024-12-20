using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        private Camera _cameraUI;
    
        private void Awake()
        {
            _cameraUI = GetComponent<Camera>();
            CameraManager.Instance.SetUICamera(_cameraUI);
        }
    }
}