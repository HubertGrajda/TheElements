using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

public class CameraAimBehaviourController : MonoBehaviour
{
    private CinemachineFreeLook _virtualCamera;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        Managers.CamerasManager.SetAimBehaviour(_virtualCamera);
    }
}