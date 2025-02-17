using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        private Camera _mainCamera;
    
        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
            CameraManager.Instance.SetMainCamera(_mainCamera);
        }
    }
}