using System.Collections;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class SmokeAdjustment : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    public VisualEffect Vfx => vfx;
    
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool canBeAffected;
    [SerializeField] private float rate = 500;
    [SerializeField] private float size = 15;
    [SerializeField] private float radius = 90;
    
    private Transform _playerTransform;
    private float _currentRotationSpeed; 
    
    private const string ROTATION_SPEED_PROPERTY = "RotationSpeed";
    private const string PLAYER_POSITION_PROPERTY = "PlayerPosition";
    
    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
        _playerTransform = PlayerManager.Instance.PlayerRef.transform;
        _currentRotationSpeed = rotationSpeed;
    }

    public void ChangeRotationSpeed(float speed)
    {
        StartCoroutine(ChangeRotationSpeedInTime(speed));
    }
    
    private void Update()
    {
        if(Vector3.Distance(_playerTransform.position, transform.position) > 20f) return;
        
        if (canBeAffected)
        {
            vfx.SetVector3(PLAYER_POSITION_PROPERTY, _playerTransform.position);
        }
    }

    private IEnumerator ChangeRotationSpeedInTime(float targetSpeed, float time = 2f)
    {
        var timer = 0f;
        var initialRotationSpeed = _currentRotationSpeed;
        
        while (timer < time)
        {
            _currentRotationSpeed = Mathf.Lerp(initialRotationSpeed, targetSpeed, timer / time);
            
            vfx.SetFloat(ROTATION_SPEED_PROPERTY, _currentRotationSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        
        _currentRotationSpeed = targetSpeed;
        vfx.SetFloat(ROTATION_SPEED_PROPERTY, _currentRotationSpeed);
    }
}

