using _Scripts.Managers;
using UnityEngine;

namespace Cameras
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