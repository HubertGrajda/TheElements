using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public class Fireball : Spell
    {
        [SerializeField] protected GameObject model;
        
        [SerializeField] private float vfxOnHitSize = 3f;
        [SerializeField] private float vfxOnHitGrowingTime = 0.2f;
        [SerializeField] private float vfxStartSize = 1f;
        [SerializeField] private float timeToDestroy = 3f;
    
        private const string VFX_SIZE_PROPERTY = "Size";
    
        private bool _exploded;

        private Rigidbody _rigidbody;
        private readonly Vector3 _offset = new(0, 0.5f, 0);
        
        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void Cast()
        {
            base.Cast();
            gameObject.SetActive(true);
            transform.SetParent(SpawnPoint);
        
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            Vfx?.Play();
        }

        public override void Launch()
        {
            base.Launch();
            _exploded = false;
            transform.SetParent(null);

            var target = SpellLauncher.GetTarget();
            var direction =  (target - transform.position + _offset).normalized;
        
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }
    
        protected void OnCollisionEnter(Collision _)
        {
            if (_exploded) return;
        
            StartCoroutine(HandleCollision());
        }
    
        private void OnCollisionEffectEnded()
        {
            gameObject.SetActive(false);
            model.SetActive(true);
            Vfx.SetFloat(VFX_SIZE_PROPERTY, vfxStartSize);
        }

        private IEnumerator HandleCollision()
        {
            Vfx?.Play();
            model.SetActive(false);
            _exploded = true;
            
            yield return ChangeSizeOverTime(vfxOnHitSize, vfxOnHitGrowingTime);
            yield return ChangeSizeOverTime(0f, timeToDestroy);
            
            OnCollisionEffectEnded();
        }
    
        private IEnumerator ChangeSizeOverTime(float finalSize, float resizingTime)
        {
            var timer = 0f;
            var initialSize = Vfx.GetFloat(VFX_SIZE_PROPERTY);
            
            while (timer < resizingTime)
            {
                var value = Mathf.Lerp(initialSize, finalSize, timer / resizingTime);
            
                Vfx.SetFloat(VFX_SIZE_PROPERTY, value);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}