using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts
{
    [RequireComponent(typeof(VisualEffect))]
    public class Smoke : MonoBehaviour, IColorProvider, IAirInteractable
    {
        [SerializeField] private bool canBeAffected;
        [SerializeField] private float rate = 500;
        [SerializeField] private float size = 15;
        [SerializeField] private float radius = 90;
    
        private VisualEffect _vfx;
        private Coroutine _changeRotationCoroutine;
        private float _currentRotationSpeed; 
    
        private const string COLOR_PARAM = "Color";
        private const string ROTATION_SPEED_PROPERTY = "RotationSpeed";
    
        private const float MAX_ROTATION_SPEED = 0.05f;
    
        private void Awake()
        {
            _vfx = GetComponent<VisualEffect>();
        }

        public Color GetColor() => _vfx.GetVector4(COLOR_PARAM);

        public void OnInteractionStart()
        {
            ChangeRotationSpeed(MAX_ROTATION_SPEED);
        }

        public void OnInteractionEnd()
        {
            ChangeRotationSpeed(0f);
        }

        public void OnInteractionStay(GameObject trigger)
        {
            transform.position = trigger.transform.position;
        }
    
        private void ChangeRotationSpeed(float value, float time = 2f)
        {
            if (_changeRotationCoroutine != null)
            {
                StopCoroutine(_changeRotationCoroutine);
            }

            _changeRotationCoroutine = StartCoroutine(ChangeRotationSpeedOverTime(value, time));
        }
    
        private IEnumerator ChangeRotationSpeedOverTime(float targetSpeed, float time)
        {
            var timer = 0f;
            var initialRotationSpeed = _currentRotationSpeed;
        
            while (timer < time)
            {
                _currentRotationSpeed = Mathf.Lerp(initialRotationSpeed, targetSpeed, timer / time);
            
                _vfx.SetFloat(ROTATION_SPEED_PROPERTY, _currentRotationSpeed);
                timer += Time.deltaTime;
                yield return null;
            }
        
            _currentRotationSpeed = targetSpeed;
            _vfx.SetFloat(ROTATION_SPEED_PROPERTY, _currentRotationSpeed);
        }
    }
}