using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleProjectile : MonoBehaviour
    {
        [SerializeField] private Collider collisionsCollider;
        private Rigidbody _rigidbody;
    
        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Prepare(Transform spawnTransform)
        {
            _rigidbody.isKinematic = true;
            collisionsCollider.enabled = false;
            transform.position = spawnTransform.position;
            transform.SetParent(spawnTransform, true);
            gameObject.SetActive(true);
        }

        public void Shoot(Vector3 force)
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(force, ForceMode.Impulse);
            collisionsCollider.enabled = true;
        }
    }
}