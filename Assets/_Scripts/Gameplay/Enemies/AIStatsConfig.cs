using UnityEngine;

namespace _Scripts.AI
{
    [CreateAssetMenu(menuName = "Stats/AIStats", fileName ="newAIStats")]
    public class AIStatsConfig : BaseStatsConfig
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }

        [field: SerializeField] public float MeleeAttackRange { get; private set; }
        [field: SerializeField] public float RangedAttackMinDistance { get; private set; }
        [field: SerializeField] public float RangedAttackMaxDistance { get; private set; }
        [field: SerializeField] public float FollowingTargetRange { get; private set; }
        [field: SerializeField] public float TriggeredDetectionRange { get; private set; }
    
        [field: SerializeField] public float ShootingFrequency { get; private set; }

        [field: SerializeField] public float MeleeAttackCooldown { get; private set; }
        
        [field: SerializeField] public float Experience { get; private set; }
    }
}