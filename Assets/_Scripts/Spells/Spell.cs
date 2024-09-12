using System.Collections;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(VisualEffect))]
    public abstract class Spell : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public SpellDataSO SpellData { get; private set; }
        
        [SerializeField] protected bool dealDamageInTime;
        [SerializeField] protected float damagingFrequency;
        [SerializeField] protected int damage;
        [SerializeField] protected float speed;
        [SerializeField] private float disableDelay;

        protected VisualEffect Vfx { get; private set; }
        protected Collider SpellCollider { get; private set; }
        
        private float _timer;

        protected virtual void Awake()
        {
            SpellCollider = GetComponent<Collider>();
            Vfx = GetComponent<VisualEffect>();
        }
        protected virtual void Start()
        {
            SpellCollider.isTrigger = true;
        }
    
        protected virtual void DealDamage(IDamageable enemy)
        {
            enemy.Damaged(damage);
        }

        protected IEnumerator DisableSpellAfterDelay()
        {
            yield return new WaitForSeconds(disableDelay );
            gameObject.SetActive(false);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.ENEMY_TAG) && !dealDamageInTime)
            {
                var enemy = other.GetComponent<IDamageable>();
                DealDamage(enemy);
            }
        }
        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.Tags.ENEMY_TAG) && dealDamageInTime)
            {
                if (_timer <= 0)
                {
                    _timer = damagingFrequency;
                    var enemy = other.GetComponent<IDamageable>();
                    DealDamage(enemy);
                }
            }
            _timer -= Time.deltaTime;
        }

        public virtual void CastSpell()
        {
            AudioManager.Instance.PlaySound(SpellData.CastingBehaviour.CastingSound);
        }

        public void OnSpawnFromPool()
        {
            throw new System.NotImplementedException(); // TODO:
        }

        public void ReturnToPool()
        {
            throw new System.NotImplementedException(); // TODO:
        }
    }
}