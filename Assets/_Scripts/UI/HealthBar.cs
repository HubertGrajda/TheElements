﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public abstract class HealthBar : MonoBehaviour
    {
        [SerializeField] private float smoothingDuration = 0.5f;

        private Slider _slider;
        private Coroutine _setHealthBarCoroutine;
        private BaseHealthSystem _healthSystem;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0;
        }

        protected void Init(BaseHealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _slider.maxValue = healthSystem.CurrentHealth;
            _slider.value = healthSystem.CurrentHealth;
            
            healthSystem.OnHealthChanged += OnHealthChanged;
            healthSystem.OnDeath += OnDeath;
        }

        private void OnHealthChanged(int value)
        {
            if (_setHealthBarCoroutine != null)
            {
                StopCoroutine(_setHealthBarCoroutine);
            }
            
            _setHealthBarCoroutine = StartCoroutine(SetHealthBar(value));
        }

        protected virtual void OnDeath()
        {
            _healthSystem.OnHealthChanged -= OnHealthChanged;
            _healthSystem.OnDeath -= OnDeath;
        }

        private IEnumerator SetHealthBar(int value)
        {
            var timer = 0f;
            var initialValue = _slider.value;
            
            while (timer <= smoothingDuration)
            {
                _slider.value = Mathf.Lerp(initialValue, value, timer/smoothingDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            _slider.value = value;
            _setHealthBarCoroutine = null;
        }
    }
}