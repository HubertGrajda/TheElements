using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Stats/PlayerStats/PlayerMovementStats")]
public class PlayerMovementStats_SO : ScriptableObject
{
    [Header("Jumping settings")]
    public int maxJumps = 2;
    public float jumpHeight = 1.5f;

    [Header("Movement settings")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float crouchSpeed = 0.7f;
    public float floatingSpeed = 0.5f;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity = 0.5f;

    [Header("Falling Settings")]
    public float groundedGravity = -4f;
    public float gravityValue = -10f;
}