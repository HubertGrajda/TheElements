using Player;
using UnityEngine;

public class RunningState : PlayerMovementState
{
    protected override bool CanBeEntered =>
        IsGrounded &&
        Fsm.IsMovingInputActive &&
        Fsm.IsRunningInputActive;

    public RunningState(PlayerMovementStateMachine fsm) : base(fsm)
    {
    }

    public override void EnterState()
    {
        Fsm.SetCurrentSpeed(Stats.RunSpeed);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        CharacterController.Move(Fsm.SetDirection() * (Fsm.CurrentSpeed * Time.deltaTime));
    }
}
