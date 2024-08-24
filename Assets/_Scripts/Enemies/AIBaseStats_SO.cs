using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Stats/AIStats", fileName ="newAIStats")]
public class AIBaseStats_SO : BaseStats
{
    [Header("SPECIFIC TO AI UNITS")]
    [Header("UI stuff")]
    public string unitName;
    public string unitDescription;

    public enum UnitType
    {
        WaterElemental,
        FireElemental,
        EarthElemental,
        AirElemental,
        Humanoid,
        Animal,
        Unique
    }
    
    public UnitType unitType;

    public float movementSpeed;

    [Header("Detection")]
    public float meleeAttackRange;
    public float rangedAttackRange;
    public float followingTargetRange;
    
    [Header("Ranged Attack")]
    public float shootingFrequency;
    public float shootingForce;
    public int shootingSeriesCount;
    public EnemyProjectile projectile;

    [Header("Melee attack")] 
    public float meleeAttackCooldown;
}
