using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(VisualEffect))]
    public abstract class Spell : MonoBehaviour, ISpell, IPoolable
    {
        [field: SerializeField] public SpellConfig SpellData { get; private set; }
        
        [SerializeField] protected float speed;

        protected VisualEffect Vfx { get; private set; }
        protected Collider SpellCollider { get; private set; }
        
        public virtual bool CanBeLaunched => true;
        public bool CanBeUsed =>
            _spellLimiter == null && !TryGetSpellLimiter(out _spellLimiter) ||
            _spellLimiter.IsLimited == false;

        protected bool Launched { get; private set; }
        protected bool Cancelled { get; private set; }
        
        private Animator _casterAnimator;
        private SpellLimiterController _spellLimiterController;
        private SpellLimiter _spellLimiter;
        
        protected virtual void Awake()
        {
            SpellCollider = GetComponent<Collider>();
            Vfx = GetComponent<VisualEffect>();
        }
        
        public virtual void Use(Animator casterAnimator)
        {
            Launched = false;
            Cancelled = false;
            
            _casterAnimator = casterAnimator;

            if (TryGetSpellLimiter(out _spellLimiter))
            {
                _spellLimiter.OnSpellUsage();
                _spellLimiter.OnBecameLimited += Cancel;
            }
            
            Cast();
        }

        public virtual void Cast()
        {
            SpellData.CastingBehaviour.StartCasting(_casterAnimator);
        }

        public virtual void PrepareToLaunch()
        {
        }
        
        public virtual void Launch()
        {
            PrepareToLaunch();
            gameObject.SetActive(true);
            Launched = true;
        }
        
        public virtual void Cancel()
        {
            if (Cancelled) return;
            
            SpellData.CastingBehaviour.StopCasting(_casterAnimator);

            if (TryGetSpellLimiter(out _spellLimiter))
            {
                _spellLimiter.OnSpellCanceled();
            }
            
            Cancelled = true;
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

        protected void OnDisable() => StopAllCoroutines();

        public bool TryGetSpellLimiter(out SpellLimiter limiter)
        {
            limiter = default;
            
            var limiterConfig = SpellData.SpellLimiterConfig;

            if (limiterConfig == null) return false;
            
            limiter = limiterConfig.GetInstance(this);
            
            return limiter != null;
        }
    }
}