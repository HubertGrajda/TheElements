using System;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellLauncher : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        public Transform SpawnPoint => spawnPoint;
    
        public Action<SpellConfig> OnSpellUsed;
    
        private Spell _spell;
        private SpellConfig _spellConfig;
        private SpellLimiter _spellLimiter;
        private ObjectPoolingManager _objectPoolingManager;

        private readonly Dictionary<SpellConfig, SpellLimiter> _spellConfigsToLimiters = new();
    
        public Animator Animator { get; private set; }
        public bool IsLaunching { get; private set; }
        private bool CanBeUsed => _spellLimiter == null || _spellLimiter.IsLimited == false;

        protected SpellCastingBehaviour CurrentCastingBehaviour => _spellConfig?.CastingBehaviour;
    
        protected void Awake()
        {
            Animator = GetComponent<Animator>();
            _objectPoolingManager = ObjectPoolingManager.Instance;
        }

        public void UseSpell()
        {
            if (!TryGetSpellToUse(out _spellConfig)) return;
        
            PrepareLimiterForSpell(_spellConfig);
        
            if (!CanBeUsed) return;
        
            _spell = _objectPoolingManager.GetFromPool(_spellConfig.SpellPrefab);
            _spell.Use(this);
        
            OnSpellUsed?.Invoke(_spellConfig);

            if (_spellLimiter != null)
            {
                _spellLimiter.OnSpellUsed();
                _spellLimiter.OnBecameLimited += OnSpellBecameLimited;
            }
        
            IsLaunching = true;
            _spellConfig.CastingBehaviour.ToggleCastingAnimation(this, true);
        }

        public void CastSpell()
        {
            if (_spellConfig == null) return;
            if (_spell.Cancelled) return;
        
            _spell.Cast();
        }

        private void PrepareLimiterForSpell(SpellConfig spellConfig)
        {
            if (TryGetLimiter(spellConfig, out _spellLimiter)) return;

            if (spellConfig.SpellLimiterConfig == null)
            {
                _spellLimiter = null;
                return;
            }
        
            _spellLimiter = spellConfig.SpellLimiterConfig.CreateLimiterInstance();
            AddLimiter(spellConfig, _spellLimiter);
        }
    
        public void LaunchSpell()
        {
            if (_spell == null) return;
            if (!_spell.CanBeLaunched) return;
        
            _spell.transform.rotation = transform.rotation;
            _spell.Launch();
        
            IsLaunching = false;
        }
    
        protected void CancelSpell()
        {
            if (_spell.Cancelled) return;
        
            _spell.Cancel();
            _spellLimiter.OnSpellCanceled();
        
            IsLaunching = false;
        }

        private void OnSpellBecameLimited()
        {
            CancelSpell();
            CurrentCastingBehaviour.ToggleCastingAnimation(this, false);
        }
    
        public bool TryGetLimiter(SpellConfig config, out SpellLimiter limiter) =>
            _spellConfigsToLimiters.TryGetValue(config, out limiter);

        private void AddLimiter(SpellConfig config, SpellLimiter limiter) => 
            _spellConfigsToLimiters.TryAdd(config, limiter);
    
        protected abstract bool TryGetSpellToUse(out SpellConfig spellConfig);
    
        public abstract Vector3 GetTarget();
    }
}