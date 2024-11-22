using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Spells
{
    public class FlameThrower : Spell
    {
        [SerializeField] private float size;
        [SerializeField] private float sizeForExperienceModifier = 0.02f;
        
        private float _currSize;
        private bool _vfxIsPlaying;

        private const string SPHERE_COLLIDER_POSITION = "SphereColliderPosition";
        private const string SIZE = "Size";

        private PlayerExperienceSystem _experienceSystem;
        private static PlayerInputs.PlayerActions Actions => InputsManager.Instance.PlayerActions;
        
        protected override void Awake()
        {
            base.Awake();
            _experienceSystem = PlayerManager.Instance.ExperienceSystem;
        }

        public override bool CanBeCasted => Actions.CastSpell.IsPressed();

        private void FixedUpdate()
        {
            if (!_vfxIsPlaying) return;
            
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);

            transform.LookAt(ray.GetPoint(100));
            
            if (Physics.Raycast(Vfx.transform.position, Vfx.transform.forward, out var hit))
            {
                Vfx.SetVector3(SPHERE_COLLIDER_POSITION, hit.point);
            }
        }

        private void OnCastSpellInput(InputAction.CallbackContext context)
        {
            if (!_vfxIsPlaying) return;
            
            FlameThrowerStop();
        }
        
        private void FlameThrowerStart()
        {
            _vfxIsPlaying = true;
            Vfx.SetFloat(SIZE, _currSize);
            Vfx.Play();
            Debug.Log("start");
        }
    
        private void FlameThrowerStop()
        {
            _vfxIsPlaying = false;
            Vfx.Stop();
            Disable();
            Debug.Log("stop");
        }
    
        public override void CastSpell()
        {
            base.CastSpell();
            FlameThrowerStart();
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            Actions.CastSpell.canceled += OnCastSpellInput;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            Actions.CastSpell.canceled -= OnCastSpellInput;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            var experience = _experienceSystem.GetExperienceValue(SpellData.ElementType);
            var additionalSize = experience * sizeForExperienceModifier;
            
            _currSize = size + additionalSize;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(Vfx.GetVector3(SPHERE_COLLIDER_POSITION), 0.4f);
        }
    }
}