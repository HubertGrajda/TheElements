using Player;
using UnityEngine;

public class WalkingState : State
{
    private readonly PlayerMovementStateMachine _fsm;
    private readonly PlayerMovementStats_SO _stats;
    public WalkingState(PlayerMovementStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
        _stats = _fsm.MovementStats;
    }

    public override void EnterState()
    {
        _fsm.SetCurrentSpeed(_stats.walkSpeed);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        _fsm.Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 0.5f, 0.1f, Time.deltaTime);
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.IsJumpingInputActive)
        {
            stateToSwitch = _fsm.JumpingState;
        }
        else if (!_fsm.IsMovingInputActive)
        {
            stateToSwitch = _fsm.IdleState;
        }
        else if (_fsm.IsRunningInputActive)
        {
            stateToSwitch = _fsm.RunningState;
        }
        else if (_fsm.IsCrouchingInputActive)
        {
            stateToSwitch = _fsm.CrouchingState;
        }
        
        return stateToSwitch != null;
    }
}
