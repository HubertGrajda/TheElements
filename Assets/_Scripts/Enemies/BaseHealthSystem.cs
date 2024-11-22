using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] protected float destructionDelay;
    [SerializeField] protected Slider healthbar;
    [SerializeField] protected BaseStats stats;

    protected bool isDead;
    protected int currentHealth;
    
    protected virtual void Start()
    {
        currentHealth = stats.maxHealth;
        healthbar.maxValue = currentHealth;
        healthbar.value = currentHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        StartCoroutine(SetHealthBar());
        
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        if (isDead) return;
        
        isDead = true;
        healthbar.gameObject.SetActive(false);
        Destroy(gameObject, destructionDelay);
    }
    
    private IEnumerator SetHealthBar()
    {
        while(healthbar.value > currentHealth)
        {
            healthbar.value -= 0.1f;
            yield return null;
        }
    }
}
