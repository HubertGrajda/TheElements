using _Scripts.Managers;
using UnityEngine;
using UnityEngine.VFX;
public class SmokeAdjustment : MonoBehaviour
{
    public float rotationSpeed = 0;
    public VisualEffect vfx;
    
    [SerializeField] private bool _canBeAffected;
    [SerializeField] private float rate = 500;
    [SerializeField] private float size = 15;
    [SerializeField] private float radius = 90;
    
    private Transform _playerTransform;
    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
        _playerTransform = Managers.PlayerManager.PlayerRef.transform;
    }

    private void Update()
    {
        if(Vector3.Distance(_playerTransform.position, transform.position) > 20f) return;
        
        vfx.SetFloat("RotationSpeed", Mathf.Lerp(vfx.GetFloat("RotationSpeed") , rotationSpeed, 0.005f));

        if (_canBeAffected)
        {
            vfx.SetVector3("PlayerPosition", _playerTransform.position);
        }
    }
}

