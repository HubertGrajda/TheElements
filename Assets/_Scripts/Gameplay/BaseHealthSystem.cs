using System;
using UnityEngine;

public  class BaseHealthSystem : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    public event Action<int> OnHealthChanged;
    
    [SerializeField] protected float destructionDelay;
    [SerializeField] protected BaseStatsConfig stats;
    
    private bool _isDead;
    public int CurrentHealth { get; private set; }

    protected virtual void Awake()
    {
        CurrentHealth = stats.maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_isDead) return;
        
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);
        
        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        if (_isDead) return;
        
        _isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject, destructionDelay);
    }
}
