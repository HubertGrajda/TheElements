using _Scripts.Cameras;
using UnityEngine;

namespace _Scripts
{
    public class LookAtMainCamera : MonoBehaviour
    {
        private Camera _camera;
        
        private void Start()
        {
            _camera = CameraManager.Instance.CameraMain;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.transform.forward);
        }
    }
}