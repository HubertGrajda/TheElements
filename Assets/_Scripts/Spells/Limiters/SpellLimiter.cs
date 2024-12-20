using System;
using System.Collections;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellLimiter
    {
        public virtual bool IsLimited => false;

        public float MaxValue { get; protected set; }
        public float CurrentValue { get; protected set; }
        public float MinValue { get; protected set; }

        public Action<float> OnCurrentValueChanged;
        public Action OnBecameLimited;
        protected SpellsManager SpellsManager { get; }
        
        protected SpellLimiter()
        {
            SpellsManager = SpellsManager.Instance;
        }
        
        public virtual void OnSpellUsed()
        {
        }

        public virtual void OnSpellPerform()
        {
        }

        public virtual void OnSpellCanceled()
        {
        }
    }
    
    public class CooldownLimiter : SpellLimiter
    {
        public override bool IsLimited => CurrentValue > MinValue;
        
        private readonly float _cooldown;
        private Action _onCooldownEnd;

        public CooldownLimiter(float cooldown)
        {
            _cooldown = cooldown;
            MaxValue = cooldown;
        }

        public override void OnSpellUsed()
        {
            SpellsManager.StartCoroutine(StartCooldown());
        }
        
        private IEnumerator StartCooldown()
        {
            CurrentValue = _cooldown;
            var timer = 0f;
            
            while (timer <= _cooldown)
            {
                timer += Time.deltaTime;
                CurrentValue = _cooldown - timer;
                OnCurrentValueChanged?.Invoke(CurrentValue);
                yield return null;
            }

            CurrentValue = 0;

            _onCooldownEnd?.Invoke();
        }
    }

    public class UsageTimeLimiter : SpellLimiter
    {
        public override bool IsLimited => CurrentValue >= MaxValue;
        
        private readonly float _maxDuration;
        
        private Coroutine _changeValueCoroutine;
        
        public UsageTimeLimiter(float maxDuration)
        {
            _maxDuration = maxDuration;
            MaxValue = maxDuration;
        }

        public override void OnSpellUsed()
        {
            base.OnSpellUsed();

            ChangeValue(_maxDuration);
        }

        private void ChangeValue(float value)
        {
            if (_changeValueCoroutine != null)
            {
                SpellsManager.StopCoroutine(_changeValueCoroutine);
            }
            
            _changeValueCoroutine = SpellsManager.StartCoroutine(ChangeValueOverTime(value));
        }
        
        private IEnumerator ChangeValueOverTime(float desiredValue)
        {
            var timer = 0f;
            var currentValue = CurrentValue;
            var timeToRequiredValue = Mathf.Abs(desiredValue - currentValue);
            
            while (timer <= timeToRequiredValue)
            {
                timer += Time.deltaTime;
                CurrentValue = Mathf.Lerp(currentValue, desiredValue, timer / timeToRequiredValue);
                OnCurrentValueChanged?.Invoke(CurrentValue);
                yield return null;
            }
            
            if (IsLimited)
            {
                OnBecameLimited?.Invoke();
            }
        }

        public override void OnSpellCanceled()
        {
            base.OnSpellCanceled();

           ChangeValue(0f);
        }
    }
}