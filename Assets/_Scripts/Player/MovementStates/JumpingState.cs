using System;
using Player;
using UnityEngine;

public class JumpingState : State
{
    private readonly PlayerMovementStateMachine _fsm;
    private readonly PlayerMovementStats_SO _stats;
    public JumpingState(PlayerMovementStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
        _stats = _fsm.MovementStats;
    }

    private JumpingSubState CurrentJumpingSubState { get; set; }

    public enum JumpingSubState // Update animator after changing
    {
        EmptyState,
        LaunchingGrounded,
        LaunchingInAir,
        RisingUp,
        Falling,
        Landing
    }

    private int _availableJumps;
    
    public override void EnterState()
    {
        _availableJumps = _stats.maxJumps;
        AddListeners();
        CurrentJumpingSubState = JumpingSubState.LaunchingGrounded;
        _fsm.Animator.SetInteger(Constants.AnimationNames.JUMP, 1);
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        switch (CurrentJumpingSubState)
        {
            case JumpingSubState.LaunchingGrounded:
            case JumpingSubState.LaunchingInAir:
            case JumpingSubState.Landing:
            {
                return;
            }
            case JumpingSubState.RisingUp:
            {
                if (_fsm.IsJumpingInputActive && _availableJumps > 0)
                {
                    SetSubState(JumpingSubState.LaunchingInAir);
                }
                
                if (_fsm.IsMovingInputActive && _fsm.CurrentSpeed < _stats.floatingSpeed)
                {
                    _fsm.SetCurrentSpeed(_stats.floatingSpeed);
                }
                
                if (_fsm.PlayerVelocity.y < -1f)
                {
                    SetSubState(JumpingSubState.Falling);
                }
                
                _fsm.AddVelocityY(_stats.gravityValue * Time.deltaTime);
                break;
            }
            case JumpingSubState.Falling:
            {
                if (_fsm.IsGrounded)
                {
                    SetSubState(JumpingSubState.Landing);
                }
                
                _fsm.AddVelocityY(_stats.gravityValue * Time.deltaTime);
                break;
            }
        }
    }

    public override void EndState()
    {
        var groundedVelocity = new Vector3(_fsm.PlayerVelocity.x, _stats.groundedGravity, _fsm.PlayerVelocity.z);
        _fsm.SetVelocity(groundedVelocity);
        RemoveListeners();
    }

    private void Jump()
    {
        _availableJumps--;

        var jumpingVelocityY = Mathf.Sqrt(_stats.jumpHeight * -3f * _stats.gravityValue);
        var jumpingVelocity = new Vector3(_fsm.PlayerVelocity.x, jumpingVelocityY, _fsm.PlayerVelocity.z);
        
        _fsm.SetVelocity(jumpingVelocity);
        
        SetSubState(JumpingSubState.RisingUp);
    }

    private void AddListeners()
    {
        _fsm.PlayerEvents.jump += Jump;
        _fsm.PlayerEvents.jumpingSubStateChanged += OnSubStateChanged;
    }

    private void RemoveListeners()
    {
        _fsm.PlayerEvents.jump -= Jump;
        _fsm.PlayerEvents.jumpingSubStateChanged -= OnSubStateChanged;
    }

    private void OnSubStateChanged(JumpingSubState subState)
    {
        _fsm.Animator.SetInteger(Constants.AnimationNames.JUMP, (int) subState);
        
        switch (subState)
        {
            case JumpingSubState.EmptyState:
                break;
            case JumpingSubState.LaunchingGrounded:
                break;
            case JumpingSubState.LaunchingInAir:
                break;
            case JumpingSubState.RisingUp:
                break;
            case JumpingSubState.Falling:
                break;
            case JumpingSubState.Landing:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(subState), subState, null);
        }
     }

    private void SetSubState(JumpingSubState subState)
    {
        CurrentJumpingSubState = subState;
        _fsm.PlayerEvents.OnJumpingSubStateChanged(CurrentJumpingSubState);
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if(!_fsm.IsGrounded || CurrentJumpingSubState != JumpingSubState.Landing) return false;
        
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
