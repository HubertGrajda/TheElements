using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public abstract class HealthBar : MonoBehaviour
    {
        [SerializeField] private float smoothingDuration = 0.5f;
        [SerializeField] protected TMP_Text healthPointsText;
        
        private Slider _slider;
        private Coroutine _setHealthBarCoroutine;
        private HealthSystem _healthSystem;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0;
        }

        protected void Init(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _slider.maxValue = healthSystem.MaxHealth;
            _slider.value = healthSystem.CurrentHealth;
            RefreshHealthText();

            healthSystem.OnHealthChanged += OnHealthChanged;
            healthSystem.OnDeath += OnDeath;
        }

        private void OnHealthChanged(int health)
        {
            RefreshHealthText();
            
            if (_setHealthBarCoroutine != null)
            {
                StopCoroutine(_setHealthBarCoroutine);
            }
            _setHealthBarCoroutine = StartCoroutine(SetHealthBar(health));
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

        private void RefreshHealthText()
        {
            if (healthPointsText == null) return;
            
            healthPointsText.text = $"{_healthSystem.CurrentHealth} / {_slider.maxValue}";
        }
    }
}