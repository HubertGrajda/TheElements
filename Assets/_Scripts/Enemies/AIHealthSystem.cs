using UnityEngine;

public enum DamageType
{
    Mechanical,
    Water,
    Air,
    Fire,
    Earth
}
public class AIHealthSystem : BaseHealthSystem
{
    private Animator _anim;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    
    public override void TakeDamage(int damage)
    {
        _anim.SetTrigger(Constants.AnimationNames.DAMAGED);
        
        base.TakeDamage(damage);
    }
    
    public override void Death()
    {
        GetComponent<AIStateMachine>().enabled = false;
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        
        base.Death();
    }

    private int CalculateFinalDamage(int damage, DamageType[] damageTypes)
    {
        var finalDamage = damage;

        foreach (var damageType in damageTypes)
        {
            if (damageType == DamageType.Air)
                finalDamage -= damage * stats.airResistance / 100;
            
            if (damageType == DamageType.Fire)
                finalDamage -= damage * stats.fireResistance / 100;
            
            if (damageType == DamageType.Water)
                finalDamage -= damage * stats.waterResistance / 100;
            
            if (damageType == DamageType.Earth)
                finalDamage -= damage * stats.earthResistance / 100;
        }

        return finalDamage;
    }
}
