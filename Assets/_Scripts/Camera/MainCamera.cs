using _Scripts.Managers;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Camera _cameraUI;
    
    private void Awake()
    {
        _cameraUI = GetComponent<Camera>();
    }

    private void Start()
    {
        Managers.CamerasManager.SetMainCamera(_cameraUI);
    }
}