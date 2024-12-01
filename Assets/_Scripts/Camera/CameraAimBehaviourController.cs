using _Scripts.Managers;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraAimBehaviourController : MonoBehaviour
{
    private CinemachineFreeLook _virtualCamera;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        CameraManager.Instance.SetAimBehaviour(_virtualCamera);
    }
}
