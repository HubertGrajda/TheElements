using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] protected float destructionDelay;
    [SerializeField] protected Slider healthbar;
    [SerializeField] protected BaseStatsConfig stats;

    protected bool IsDead { get; private set; }
    
    private int _currentHealth;
    
    protected virtual void Start()
    {
        _currentHealth = stats.maxHealth;
        healthbar.maxValue = _currentHealth;
        healthbar.value = _currentHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead) return;
        
        _currentHealth -= damage;
        StartCoroutine(SetHealthBar());
        
        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        if (IsDead) return;
        
        IsDead = true;
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
