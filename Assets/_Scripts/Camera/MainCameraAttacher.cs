using _Scripts.Managers;
using UnityEngine;

namespace Cameras
{
    [RequireComponent(typeof(Canvas))]
    public class MainCameraAttacher : MonoBehaviour
    {
        private void Awake()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = CameraManager.Instance.CameraMain;
        }
    }
}