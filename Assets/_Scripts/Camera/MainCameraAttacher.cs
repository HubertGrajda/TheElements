using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Cameras
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