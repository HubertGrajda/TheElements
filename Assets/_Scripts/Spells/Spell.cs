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
        [field: SerializeField] public SpellConfig SpellData { get; private set; }
        
        [SerializeField] protected float speed;

        protected VisualEffect Vfx { get; private set; }
        protected Collider SpellCollider { get; private set; }
        public virtual bool CanBeCasted => true;
        
        protected virtual void Awake()
        {
            SpellCollider = GetComponent<Collider>();
            Vfx = GetComponent<VisualEffect>();
        }
        
        protected virtual void Start()
        {
            SpellCollider.isTrigger = true;
        }

        protected void Disable()
        {
            if (!gameObject.activeInHierarchy) return;
            
            StartCoroutine(DisableWithLastParticle());
        }
        
        private IEnumerator DisableWithLastParticle()
        {
            yield return new WaitWhile(() => Vfx.aliveParticleCount > 1);
            gameObject.SetActive(false);
        }

        public virtual void CastSpell()
        {
            gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(SpellData.CastingBehaviour.CastingSound);
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            RemoveListeners();
        }

        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void RemoveListeners()
        {
        }

        protected virtual void AddListeners()
        {
        }

        public virtual void OnGetFromPool()
        {
        }
    }
}