using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

public class MainCameraBehaviour : MonoBehaviour //TODO: Temp solution
{
    private CinemachineFreeLook _virtualCamera;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        Managers.CamerasManager.SetMainBehaviour(_virtualCamera);
    }
}