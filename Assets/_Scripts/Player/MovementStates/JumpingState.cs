using Player;
using UnityEngine;

public class JumpingState : PlayerMovementState
{
    protected override bool CanBeEntered => 
        !IsGrounded || 
        (IsGrounded && Fsm.IsJumpingInputActive);
    
    protected override bool CanBeEnded =>
        IsGrounded && CurrentJumpingSubState == JumpingSubState.Landing;

    public JumpingState(PlayerMovementStateMachine fsm) : base(fsm)
    {
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
        _availableJumps = Stats.MaxJumps;
        CurrentJumpingSubState = IsGrounded ? JumpingSubState.LaunchingGrounded : JumpingSubState.Falling;
        SetSubState(CurrentJumpingSubState);
        AddListeners();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        CharacterController.Move(Fsm.SetDirection() * (Fsm.CurrentSpeed * Time.deltaTime));
        
        switch (CurrentJumpingSubState)
        {
            case JumpingSubState.RisingUp:
            {
                if (Fsm.IsJumpingInputActive && _availableJumps > 0)
                {
                    SetSubState(JumpingSubState.LaunchingInAir);
                }
                
                if (Fsm.PlayerVelocity.y < -1f)
                {
                    SetSubState(JumpingSubState.Falling);
                }

                break;
            }
            case JumpingSubState.Falling:
            {
                if (IsGrounded)
                {
                    SetSubState(JumpingSubState.Landing);
                }

                break;
            }
            case JumpingSubState.LaunchingGrounded:
            case JumpingSubState.LaunchingInAir:
            case JumpingSubState.Landing:
            default:
            {
                return;
            }
        }

        Fsm.AddVelocityY(Stats.GravityValue * Time.deltaTime);
    }

    public override void EndState()
    {
        var groundedVelocity = new Vector3(Fsm.PlayerVelocity.x, Stats.GroundedGravity, Fsm.PlayerVelocity.z);
        Fsm.SetVelocity(groundedVelocity);
        RemoveListeners();
    }

    private void Jump()
    {
        _availableJumps--;

        var jumpingVelocityY = Mathf.Sqrt(Stats.JumpHeight * -3f * Stats.GravityValue);
        var jumpingVelocity = new Vector3(Fsm.PlayerVelocity.x, jumpingVelocityY, Fsm.PlayerVelocity.z);
        
        Fsm.SetVelocity(jumpingVelocity);
        
        SetSubState(JumpingSubState.RisingUp);
    }

    private void AddListeners()
    {
        Fsm.PlayerEvents.Jump += Jump;
    }

    private void RemoveListeners()
    {
        Fsm.PlayerEvents.Jump -= Jump;
    }

    private void SetSubState(JumpingSubState subState)
    {
        CurrentJumpingSubState = subState;
        Fsm.OnJumpingSubStateChanged?.Invoke(subState);
    }
}