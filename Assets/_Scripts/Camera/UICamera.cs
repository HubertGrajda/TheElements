using _Scripts.Managers;
using UnityEngine;

namespace Cameras
{
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        private Camera _cameraUI;
    
        private void Awake()
        {
            _cameraUI = GetComponent<Camera>();
        }

        private void Start()
        {
            CameraManager.Instance.SetUICamera(_cameraUI);
        }
    }
}