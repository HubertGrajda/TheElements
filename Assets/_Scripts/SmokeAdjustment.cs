using System.Collections;
using _Scripts;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class SmokeAdjustment : MonoBehaviour, IColorProvider, IAirInteractable
{
    [SerializeField] private VisualEffect vfx;
    
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool canBeAffected;
    [SerializeField] private float rate = 500;
    [SerializeField] private float size = 15;
    [SerializeField] private float radius = 90;
    
    private Transform _playerTransform;
    private Coroutine _changeRotationCoroutine;
    private float _currentRotationSpeed; 
    
    private const string COLOR_PARAM = "Color";
    private const string ROTATION_SPEED_PROPERTY = "RotationSpeed";
    private const string PLAYER_POSITION_PROPERTY = "PlayerPosition";
    
    private const float MAX_ROTATION_SPEED = 0.05f;

    
    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.PlayerTransform;
        _currentRotationSpeed = rotationSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(_playerTransform.position, transform.position) > 20f) return;
        
        if (canBeAffected)
        {
            vfx.SetVector3(PLAYER_POSITION_PROPERTY, _playerTransform.position);
        }
    }


    public Color GetColor() => vfx.GetVector4(COLOR_PARAM);

    public void OnInteractionStart()
    {
        ChangeRotationSpeed(MAX_ROTATION_SPEED);
    }

    public void OnInteractionEnd()
    {
        ChangeRotationSpeed(0f);
    }
    
    private void ChangeRotationSpeed(float value, float time = 2f)
    {
        if (_changeRotationCoroutine != null)
        {
            StopCoroutine(_changeRotationCoroutine);
        }

        StartCoroutine(ChangeRotationSpeedOverTime(value, time));
    }
    
    private IEnumerator ChangeRotationSpeedOverTime(float targetSpeed, float time)
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

