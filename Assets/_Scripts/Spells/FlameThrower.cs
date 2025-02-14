using _Scripts.Managers;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.Spells
{
    public class FlameThrower : LongSpell
    {
        [SerializeField] private float size;
        [SerializeField] private float sizeForExperienceModifier = 0.02f;
        
        private float _currSize;
        private bool _vfxIsPlaying;

        private const string SPHERE_COLLIDER_POSITION = "SphereColliderPosition";
        private const string SIZE = "Size";

        private PlayerExperienceSystem _experienceSystem;
        
        protected override void Awake()
        {
            base.Awake();
            PlayerManager.Instance.TryGetPlayerComponent(out _experienceSystem);
        }

        protected override void Perform()
        {
            base.Perform();

            transform.LookAt(SpellLauncher.GetTarget());
            
            if (Physics.Raycast(Vfx.transform.position, Vfx.transform.forward, out var hit))
            {
                Vfx.SetVector3(SPHERE_COLLIDER_POSITION, hit.point);
            }
        }

        public override void PrepareToLaunch()
        {
            base.PrepareToLaunch();
            
            var experience = _experienceSystem.GetCurrentExperience(SpellData.ElementType);
            var additionalSize = experience * sizeForExperienceModifier;
            _currSize = size + additionalSize;
            Vfx.SetFloat(SIZE, _currSize);
        }

        public override void Launch()
        {
            base.Launch();
            
            Vfx.Play();
        }
        
        public override void Cancel()
        {
            if (Cancelled) return;
            
            base.Cancel();
            
            if (!Launched) return;
            Vfx.Stop();
            Disable();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(Vfx.GetVector3(SPHERE_COLLIDER_POSITION), 0.4f);
        }
    }
}