using UnityEngine;

public abstract class BaseStats : ScriptableObject
{
    public int maxHealth;
    
    [Header("Resistances in %")]

    [Range(0f, 100f)] public int waterResistance;
    [Range(0f, 100f)] public int fireResistance;
    [Range(0f, 100f)] public int earthResistance;
    [Range(0f, 100f)] public int airResistance;

    public float defence;
}
