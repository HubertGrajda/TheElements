using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class MainCameraBehaviour : MonoBehaviour //TODO: Temp solution
{
    private CinemachineFreeLook _virtualCamera;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        CameraManager.Instance.SetMainBehaviour(_virtualCamera);
    }
    
    
}
