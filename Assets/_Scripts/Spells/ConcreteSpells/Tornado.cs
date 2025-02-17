using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    public class Tornado : LongSpell
    {
        [SerializeField] private float minSize = 60f;
        [SerializeField] private float maxSize = 150f;
        [SerializeField] private float growingDuration = 5f;
        [SerializeField] private float sizeForExperienceModifier = 0.5f;
        
        [SerializeField] private float dissolvingDuration = 2f;
        
        [ColorUsage(true, true)]
        [SerializeField] private Color basicColor;
        [SerializeField] private float colorTransitionDuration = 2f;

        [SerializeField] private float maxDistance = 30f; 
        [SerializeField] private float maxDissolve = 0.6f;

        [SerializeField] private LayerMask ground;
        
        private float _groundPosY;
        private float _currMaxSize;
        private float _currSize;
        
        private const float MIN_DISSOLVE = 0f;
        private const float MAX_SNAP_DISTANCE = 50f;

        private const string COLOR_PARAM = "Color";
        private const string DISSOLVE_PARAM = "Dissolve";
        private const string SIZE_PARAM = "Size";
        private const string STOP_EVENT_NAME = "OnStop";
        
        private Coroutine _changeColorCoroutine;
        private Coroutine _dissolveCoroutine;

        protected override void Perform()
        {
            base.Perform();
            
            Movement();
            SnapToGround();
        }

        public override void PrepareToLaunch()
        {
            base.PrepareToLaunch();
            
            _currSize = minSize;
            _currMaxSize = maxSize;
            
            Vfx.SetFloat(DISSOLVE_PARAM, MIN_DISSOLVE);
            Vfx.SetFloat(SIZE_PARAM, _currSize);
            Vfx.SetVector4(COLOR_PARAM, basicColor);
            
            var target = SpellLauncher.GetTarget();
            transform.position = new Vector3(target.x, _groundPosY, target.z);
        }

        public override void Launch()
        {
            base.Launch();
            
            DissolveToValue(maxDissolve);
            StartCoroutine(ChangeFloatParamOverTime(SIZE_PARAM,growingDuration, _currMaxSize));
        }

        public override void Cancel()
        {
            if (Cancelled) return;
            
            base.Cancel();

            if (!Launched) return;
            
            Vfx.SendEvent(STOP_EVENT_NAME);
            DissolveToValue(MIN_DISSOLVE);
            Disable();
        }

        private void DissolveToValue(float desiredValue)
        {
            if (_dissolveCoroutine != null)
            {
                StopCoroutine(_dissolveCoroutine);
            }
            
            _dissolveCoroutine = StartCoroutine(ChangeFloatParamOverTime(
                DISSOLVE_PARAM,dissolvingDuration, desiredValue));
        }

        private IEnumerator ChangeFloatParamOverTime(string param, float time, float desiredValue)
        {
            var timer = 0f;
            var initialValue = Vfx.GetFloat(param);
            
            while (timer < time)
            {
                var value = Mathf.Lerp(initialValue, desiredValue, timer / time);
                Vfx.SetFloat(param, value);
                timer += Time.deltaTime;
                yield return null;
            }
            
            Vfx.SetFloat(param, desiredValue);
        }
        
        private IEnumerator ChangeColorOverTime(float time, Color desiredColor)
        {
            var timer = 0f;
            var initialValue = Vfx.GetVector4(COLOR_PARAM);
            
            while (timer < time)
            {
                var value = Vector4.Lerp(initialValue, desiredColor, timer / time);
                Vfx.SetVector4(COLOR_PARAM, value);
                timer += Time.deltaTime;
                yield return null;
            }
            
            Vfx.SetVector4(COLOR_PARAM, desiredColor);
            _changeColorCoroutine = null;
        }

        private void Movement()
        {
            var target = SpellLauncher.GetTarget();
            var direction = (target - transform.position).normalized;
            direction.y = 0;
            
            transform.position += direction * (speed * Time.deltaTime);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IColorProvider colorProvider))
            {
                var desiredColor = colorProvider.GetColor();
                ChangeColor(colorTransitionDuration, desiredColor);
            }
        }
        
        protected void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IColorProvider _))
            {
                ChangeColor(colorTransitionDuration, basicColor);
            }
        }

        private void ChangeColor(float time, Color desiredColor)
        {
            if (_changeColorCoroutine != null)
            {
                StopCoroutine(_changeColorCoroutine);
            }
                
            _changeColorCoroutine = StartCoroutine(ChangeColorOverTime(time, desiredColor));
        }

        private void SnapToGround()
        {
            var position = transform.position;

            const float yOffset = 3f;
            var origin = new Vector3(position.x, position.y + yOffset, position.z);

            if (!Physics.Raycast(origin, transform.TransformDirection(-Vector3.up), out var hit, MAX_SNAP_DISTANCE, ground)) return;
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, hit.point.y, position.z), 0.05f);
            _groundPosY = hit.point.y;
        }
    }
}