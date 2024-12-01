using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private Camera _mainCamera;
    
    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        CameraManager.Instance.SetMainCamera(_mainCamera);
    }
}
