using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        public event Action OnDeath;
        public event Action<int> OnDamaged;
    
        [SerializeField] protected float destructionDelay;
        [SerializeField] protected BaseStatsConfig stats;

        public Dictionary<ElementType, int> ElementTypeToDamageTaken { get; } = new();

        private bool _isDead;
        public int CurrentHealth { get; private set; }

        protected void Awake()
        {
            CurrentHealth = stats.maxHealth;
        }

        public void TakeDamage(int damage, ElementType elementType)
        {
            if (_isDead) return;
        
            CurrentHealth -= damage;
            OnDamaged?.Invoke(damage);
            
            ElementTypeToDamageTaken[elementType] = ElementTypeToDamageTaken.GetValueOrDefault(elementType) + damage;
        
            if (CurrentHealth <= 0)
            {
                Death();
            }
        }

        public void Death()
        {
            if (_isDead) return;
        
            _isDead = true;
            OnDeath?.Invoke();
            Destroy(gameObject, destructionDelay);
        }
    }
}