using _Scripts.Managers;
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
            _experienceSystem = PlayerManager.Instance.ExperienceSystem;
        }

        protected override void Perform()
        {
            base.Perform();
            
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);

            transform.LookAt(ray.GetPoint(100));
            
            if (Physics.Raycast(Vfx.transform.position, Vfx.transform.forward, out var hit))
            {
                Vfx.SetVector3(SPHERE_COLLIDER_POSITION, hit.point);
            }
        }

        public override void PrepareToLaunch()
        {
            base.PrepareToLaunch();
            
            var experience = _experienceSystem.GetExperienceValue(SpellData.ElementType);
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
            
            Vfx.Stop();
            Disable();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(Vfx.GetVector3(SPHERE_COLLIDER_POSITION), 0.4f);
        }
    }
}