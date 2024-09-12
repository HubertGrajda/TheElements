using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public class FlameThrower : FireSpell
    {
        [SerializeField] private float size;

        private float _currSize;
        private bool _vfxIsPlaying;
    
        protected override void Awake()
        {
            Cursor.visible = false;
            base.Awake();
        }

        private void Update()
        {
            if (_vfxIsPlaying && !Input.GetKey(KeyCode.Mouse0))
                FlameThrowerStop();
        }
    
        private void OnEnable()
        {
            if (!PlayerManager.Instance.PlayerRef.TryGetComponent(out PlayerExperienceSystem experienceSystem)) return;
            if (!experienceSystem.TryGetExperienceValue(SpellData.ElementType, out var experience)) return;
            _currSize = size + experience/ 200f;
        }

        private void FixedUpdate()
        {
            if (!_vfxIsPlaying) return;
            
            if (Physics.Raycast(Vfx.transform.position, Vfx.transform.forward, out var hit))
            {
                Vfx.SetVector3("SphereColliderPosition", hit.point);
            }
        }
    
        private void FlameThrowerStart()
        {
            _vfxIsPlaying = true;
            Vfx.SetFloat("Size", _currSize);
            Vfx.Play();
        }
    
        private void FlameThrowerStop()
        {
            _vfxIsPlaying = false;
            Vfx.Stop();
            StartCoroutine(DisableSpellAfterDelay());
        }
    
        public override void CastSpell()
        {
            base.CastSpell();
            FlameThrowerStart();
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(Vfx.GetVector3("SphereColliderPosition"), 0.4f);
        }
    }
}