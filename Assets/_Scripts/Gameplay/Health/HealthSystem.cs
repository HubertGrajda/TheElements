using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts
{
    public class HealthSystem : MonoBehaviour, IDamageable, ISaveable<HealthData>
    {
        public event Action OnDeath;
        public event Action<int> OnDamaged;
        public event Action<int> OnHealthChanged;
    
        [SerializeField] protected float destructionDelay;
        [SerializeField] protected BaseStatsConfig stats;

        public Dictionary<ElementType, int> ElementTypeToDamageTaken { get; } = new();

        private bool _isDead;
        public int CurrentHealth { get; private set; }
        public int MaxHealth => stats.MaxHealth;

        private bool _initialized;

        protected void Awake()
        {
            if (_initialized) return;
            
            CurrentHealth = MaxHealth;
            _initialized = true;
        }

        public void TakeDamage(int damage, ElementType elementType)
        {
            if (_isDead) return;
        
            SetHealth(CurrentHealth - damage);
            OnDamaged?.Invoke(damage);
            
            ElementTypeToDamageTaken[elementType] = ElementTypeToDamageTaken.GetValueOrDefault(elementType) + damage;
        
            if (CurrentHealth <= 0)
            {
                Death();
            }
        }

        private void SetHealth(int health)
        {
            if (CurrentHealth == health) return;
            
            CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth);
        }
        
        public void Death()
        {
            if (_isDead) return;
        
            _isDead = true;
            OnDeath?.Invoke();
            Destroy(gameObject, destructionDelay);
        }

        public SaveData Save() => new HealthData(CurrentHealth, _isDead);

        public void Load(HealthData data)
        {
            if (!data.TryGetData(out var currentHealth, out var isDead)) return;

            if (isDead)
            {
                _isDead = true;
                Destroy(gameObject);
                return;
            }
            
            SetHealth(currentHealth);
            _initialized = true;
        }
    }

    public class HealthData : SaveData
    {
        [JsonProperty] private bool _isDead;
        [JsonProperty] private int _currentHealth;

        public HealthData(int currentHealth, bool isDead)
        {
            _currentHealth = currentHealth;
            _isDead = isDead;
        }

        public bool TryGetData(out int currentHealth, out bool isDead)
        {
            currentHealth = _currentHealth;
            isDead = _isDead;
            return true;
        }
    }
}