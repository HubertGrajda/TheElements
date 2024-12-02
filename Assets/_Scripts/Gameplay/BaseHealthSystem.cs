using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public  class BaseHealthSystem : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    
    [SerializeField] protected float destructionDelay;
    [SerializeField] protected Slider healthbar;
    [SerializeField] protected BaseStatsConfig stats;
    
    private bool _isDead;
    private int _currentHealth;
    
    protected virtual void Start()
    {
        _currentHealth = stats.maxHealth;
        healthbar.maxValue = _currentHealth;
        healthbar.value = _currentHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        StartCoroutine(SetHealthBar());
        
        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        if (_isDead) return;
        
        _isDead = true;
        OnDeath?.Invoke();
        healthbar.gameObject.SetActive(false);
        Destroy(gameObject, destructionDelay);
    }
    
    private IEnumerator SetHealthBar()
    {
        while (healthbar.value > _currentHealth)
        {
            healthbar.value -= 0.1f;
            yield return null;
        }
    }
}
