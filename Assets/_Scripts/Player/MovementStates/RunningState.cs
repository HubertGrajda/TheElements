using Player;
using UnityEngine;

public class RunningState : State
{
    private readonly PlayerMovementStateMachine _fsm;
    private readonly PlayerMovementStats_SO _stats;
    public RunningState(PlayerMovementStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
        _stats = _fsm.MovementStats;
    }


    public override void EnterState()
    {
        _fsm.SetCurrentSpeed(_stats.runSpeed);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        _fsm.Anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 1f, 0.1f, Time.deltaTime);
    }

    public override void EndState()
    {
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.IsJumpingInputActive)
        {
            stateToSwitch =_fsm.JumpingState;
        }
        
        if (_fsm.IsMovingInputActive)
        {
            if (!_fsm.IsRunningInputActive)
            {
                stateToSwitch = _fsm.WalkingState;
            }
        }
        else
        {
            if (_fsm.IsCrouchingInputActive)
            {
                stateToSwitch = _fsm.CrouchingState;
            }
            else
            {
                stateToSwitch = _fsm.IdleState;
            }
        }

        return stateToSwitch != null;
    }
}
