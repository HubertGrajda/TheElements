using Player;
using UnityEngine;

public class IdleState : PlayerMovementState
{
    protected override bool CanBeEntered =>
        !Fsm.IsCrouchingInputActive &&
        !Fsm.IsMovingInputActive &&
        IsGrounded;
    
    public IdleState(PlayerMovementStateMachine fsm) : base(fsm)
    {
    }
    
    public override void EnterState()
    {
        var groundedVelocity = new Vector3(Fsm.PlayerVelocity.x, Stats.GroundedGravity, Fsm.PlayerVelocity.z);
        Fsm.SetVelocity(groundedVelocity);
        Fsm.SetCurrentSpeed(0f);
    }
}
