using Player;
using UnityEngine;

public class CrouchingState : State
{
    private readonly PlayerMovementStateMachine _fsm;
    private readonly PlayerMovementStatsConfig _stats;
    public CrouchingState(PlayerMovementStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
        _stats = _fsm.MovementStats;
    }
    
    public override void EnterState()
    {
        _fsm.Animator.SetBool(Constants.AnimationNames.CROUCH, true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_fsm.IsMovingInputActive)
        {
            _fsm.SetCurrentSpeed(_stats.crouchSpeed);
            _fsm.Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, _stats.crouchSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            _fsm.SetCurrentSpeed(0f);
        }
    }

    public override void EndState()
    {
        _fsm.Animator.SetBool(Constants.AnimationNames.CROUCH, false);
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.IsMovingInputActive && !_fsm.IsCrouchingInputActive)
        {
            if (_fsm.IsRunningInputActive)
            {
                stateToSwitch = _fsm.RunningState;
            }
            else
            {
                stateToSwitch = _fsm.WalkingState;
            }
        }
        else
        {
            if (!_fsm.IsCrouchingInputActive)
            {
                stateToSwitch = _fsm.IdleState;
            }
        }

        return stateToSwitch != null;
    }
}
