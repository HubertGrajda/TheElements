using UnityEngine;
namespace _Scripts.Spells
{
    public class Tornado : AirSpell
    {
        [SerializeField] private float dissolve = 0;
        [SerializeField] private float minSize = 60f;
        [SerializeField] private float maxSize = 150f;
        [SerializeField] private float growingSpeed = 0.01f;

        [ColorUsage(true, true)]
        [SerializeField] private Color basicColor;

        private float _groundPosY;
        private float _currSize;
        private bool _colorIsChanged;
        private bool _destructed;

        private const string COLOR_PARAM = "Color";
        private const string DISSOLVE_PARAM = "Dissolve";
        private const string SIZE_PARAM = "Size";
        private const string WATER_COLOR_SHADER_PARAM = "_WaterColor";

        private void Update()
        {
        
            if(!Input.GetKey(KeyCode.Mouse0))
            {
                _destructed = true;
                DestroyAfterDissolving();
            }

            if(!_colorIsChanged)
            {
                vfx.SetVector4(COLOR_PARAM, Vector4.Lerp(vfx.GetVector4(COLOR_PARAM), basicColor, 0.01f));
            }    
        
            if (!_destructed)
            {
                Movement();
                _groundPosY = SnapToGround();
            }
        }

        private void OnEnable()
        {
            _currSize = minSize;
            _destructed = false;
            spellCollider.enabled = true;

            if (!Managers.Managers.PlayerManager.PlayerRef.TryGetComponent(out PlayerExperienceSystem experienceSystem)) return;
            if (!experienceSystem.TryGetExperienceValue(SpellData.ElementType, out var experience)) return;
            // TODO: refactor exp system
            
            _currSize += experience;
            maxSize += 2 * experience;
            vfx.SetFloat(SIZE_PARAM, _currSize);
            vfx.SetFloat(DISSOLVE_PARAM, dissolve);
            vfx.SetVector4(COLOR_PARAM, basicColor);
        }
        private void FixedUpdate()
        {
            if (dissolve < 0.55f && _destructed == false)
            {
                dissolve += 0.005f;
                vfx.SetFloat(DISSOLVE_PARAM, dissolve);
            }

            if (_destructed && dissolve > 0)
            {
                dissolve -= 0.005f;
                vfx.SetFloat(DISSOLVE_PARAM, dissolve);
            }

            if (_currSize < maxSize && _destructed == false)
            {
                _currSize += growingSpeed;
                vfx.SetFloat(SIZE_PARAM, _currSize);
            }
        }

        private void Movement()
        {
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = Camera.main.ScreenPointToRay(screenCenter);
        
            if (Physics.Raycast(ray, out var hit))
            {
                var direction = (hit.point - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * (speed * Time.deltaTime);
            }
        }

        private void DestroyAfterDissolving()
        {
            vfx.SendEvent("OnStop");
            _destructed = true;
            StartCoroutine(DisableSpellAfterDelay());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Constants.Tags.WATER_TAG))
            {
                _colorIsChanged = true;
            }
        }
        protected override void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.Tags.WATER_TAG))
            {
                _colorIsChanged = true;
                vfx.SetVector4(COLOR_PARAM, Vector4.Lerp(vfx.GetVector4(COLOR_PARAM), other.GetComponent<MeshRenderer>().material.GetColor(WATER_COLOR_SHADER_PARAM), 0.01f));
            
            }
            else if( other.CompareTag(Constants.Tags.SMOKE_TAG))
            {
                var smoke = other.GetComponent<SmokeAdjustment>();
                other.transform.position = Vector3.Lerp(other.transform.position, transform.position, 0.1f);
                _colorIsChanged = true;
                vfx.SetVector4(COLOR_PARAM, Vector4.Lerp(vfx.GetVector4(COLOR_PARAM), smoke.vfx.GetVector4(COLOR_PARAM), 0.01f));
                smoke.rotationSpeed = _destructed ? 0f : 0.05f;
            }
            base.OnTriggerStay(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _colorIsChanged = false;
        }

        public override void CastSpell()
        {
            base.CastSpell();
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = Camera.main.ScreenPointToRay(screenCenter);
        
            transform.position = Physics.Raycast(ray, out var hit) 
                ? new Vector3(hit.point.x, _groundPosY, hit.point.z)
                : new Vector3(ray.GetPoint(5).x , _groundPosY, ray.GetPoint(5).z);
        }

        private float SnapToGround()
        {
            var position = transform.position;
            var distance = new Vector3(position.x, position.y + 3, position.z);
        
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out var hit, LayerMask.NameToLayer("Ground")))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, hit.point.y, position.z), 0.05f);
                return hit.point.y;
            }
        
            return transform.position.y;

        }
    }
}