using UnityEngine;

namespace _Scripts.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Stats/PlayerStats/PlayerMovementStats")]
    public class PlayerMovementStatsConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxJumps { get; private set; } = 2;
        [field: SerializeField] public float JumpHeight { get; private set; } = 1.5f;
    
        [field: SerializeField] public float WalkSpeed { get; private set; } = 1.5f;
        [field: SerializeField] public float RunSpeed { get; private set; } = 3f;
        [field: SerializeField] public float RunMaxSpeed { get; private set; } = 6f;
        [field: SerializeField] public float RunAccelerationTime { get; private set; } = 10f;
    
        [field: SerializeField] public float CrouchSpeed { get; private set; } = 0.7f;
        [field: SerializeField] public float AccelerationSpeed { get; private set; } = 6f;
        [field: SerializeField] public float TurnSmoothTime { get; private set; } = 0.1f;

        [field: SerializeField] public float GroundedGravity { get; private set; } = -4f;
        [field: SerializeField] public float GravityValue { get; private set; } = -10f;
    }
}