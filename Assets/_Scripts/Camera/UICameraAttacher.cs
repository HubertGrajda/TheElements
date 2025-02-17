using UnityEngine;

namespace _Scripts.Cameras
{
    [RequireComponent(typeof(Canvas))]
    public class UICameraAttacher : MonoBehaviour
    {
        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = CameraManager.Instance.CameraUI;
        }
    }
}