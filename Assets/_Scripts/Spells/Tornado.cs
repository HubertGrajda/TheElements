using System.Collections;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Spells
{
    public class Tornado : Spell
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

        private bool _destructed;
        
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
        public override bool CanBeCasted => Actions.CastSpell.IsPressed();

        private PlayerInputs.PlayerActions Actions => InputsManager.Instance.PlayerActions;
        private PlayerExperienceSystem _experienceSystem;
        
        protected override void Awake()
        {
            base.Awake();
            _experienceSystem = PlayerManager.Instance.ExperienceSystem;
        }
        
        private void Update()
        {
            if (_destructed) return;
            
            Movement();
            SnapToGround();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _currSize = minSize;
            _destructed = false;
            SpellCollider.enabled = true;
            
            var experience = _experienceSystem.GetExperienceValue(SpellData.ElementType);
            var additionalMaxSize = experience * sizeForExperienceModifier;
            _currMaxSize = maxSize + additionalMaxSize;
            
            Vfx.SetFloat(DISSOLVE_PARAM, MIN_DISSOLVE);
            Vfx.SetFloat(SIZE_PARAM, _currSize);
            Vfx.SetVector4(COLOR_PARAM, basicColor);
            
            AddListeners();
        }

        protected override void AddListeners()
        {
            Actions.CastSpell.canceled += OnCastSpellInputCanceled;
        }
        
        protected override void RemoveListeners()
        {
            Actions.CastSpell.canceled -= OnCastSpellInputCanceled;
        }
        
        private void OnCastSpellInputCanceled(InputAction.CallbackContext _)
        {
            _destructed = true;

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
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            var direction = (hit.point - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * (speed * Time.deltaTime);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IColorProvider colorProvider))
            {
                var desiredColor = colorProvider.GetColor();
                _changeColorCoroutine = StartCoroutine(ChangeColorOverTime(colorTransitionDuration, desiredColor));
            }
        }
        
        protected void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IColorProvider _))
            {
                if (_changeColorCoroutine != null)
                {
                    StopCoroutine(_changeColorCoroutine);
                }
                
                _changeColorCoroutine = StartCoroutine(ChangeColorOverTime(colorTransitionDuration, basicColor));
            }
        }

        public override void CastSpell()
        {
            base.CastSpell();
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);
            
            transform.position = Physics.Raycast(ray, out var hit, maxDistance) 
                ? new Vector3(hit.point.x, _groundPosY, hit.point.z)
                : new Vector3(ray.GetPoint(maxDistance).x , _groundPosY, ray.GetPoint(maxDistance).z);
            
            DissolveToValue(maxDissolve);
            StartCoroutine(ChangeFloatParamOverTime(SIZE_PARAM,growingDuration, _currMaxSize));
        }

        private void SnapToGround()
        {
            var position = transform.position;

            const float yOffset = 3f;
            var origin = new Vector3(position.x, position.y + yOffset, position.z);

            if (!Physics.Raycast(origin, transform.TransformDirection(-Vector3.up), out var hit, MAX_SNAP_DISTANCE))
            {
                _groundPosY = transform.position.y;
                return;
            }
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, hit.point.y, position.z), 0.05f);
            _groundPosY = hit.point.y;
        }
    }
}