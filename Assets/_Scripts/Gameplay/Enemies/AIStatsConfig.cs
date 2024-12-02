using UnityEngine;

[CreateAssetMenu(menuName = "Stats/AIStats", fileName ="newAIStats")]
public class AIStatsConfig : BaseStatsConfig
{
    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public float MeleeAttackRange { get; private set; }
    [field: SerializeField] public float RangedAttackRange { get; private set; }
    [field: SerializeField] public float FollowingTargetRange { get; private set; }
    
    [field: SerializeField] public float ShootingFrequency { get; private set; }
    [field: SerializeField] public float ShootingForce { get; private set; }
    [field: SerializeField] public EnemyProjectile Projectile { get; private set; }

    [field: SerializeField] public float MeleeAttackCooldown { get; private set; }
}
