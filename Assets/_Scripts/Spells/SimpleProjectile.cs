using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private Collider collisionsCollider;
        [SerializeField] private float lifeTime = 4f;
        
        private Rigidbody _rigidbody;
        private Collider _triggerCollider;
    
        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _triggerCollider = GetComponent<Collider>();
        }

        public void Prepare(SpellLauncher spellLauncher)
        {
            _rigidbody.isKinematic = true;
            _triggerCollider.excludeLayers = spellLauncher.ExcludeLayerMask;
            
            collisionsCollider.enabled = false;
            transform.position = spellLauncher.SpawnPoint.position;
            transform.SetParent(spellLauncher.SpawnPoint, true);
            gameObject.SetActive(true);
        }

        public void Shoot(Vector3 force)
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            collisionsCollider.enabled = true;
            Invoke(nameof(Disable), lifeTime);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}