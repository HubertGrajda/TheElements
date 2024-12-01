using Player;
using UnityEngine;

public class IdleState : State
{
    private readonly PlayerMovementStateMachine _fsm;
    public IdleState(PlayerMovementStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
    }
    
    public override void EnterState()
    {
        _fsm.SetCurrentSpeed(0f);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        _fsm.Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 0f, 0.1f, Time.deltaTime);
        if(_fsm.Animator.GetFloat(Constants.AnimationNames.MOVEMENT_SPEED) < 0.001f)
        {
            _fsm.Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 0f);
        }
    }

    public override void EndState()
    {
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.IsJumpingInputActive)
        {
            stateToSwitch = _fsm.JumpingState;
        }
        
        if (_fsm.IsMovingInputActive)
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
        
        if (_fsm.IsCrouchingInputActive)
        {
            stateToSwitch = _fsm.CrouchingState;
        }

        return stateToSwitch != null;
    }
}
