using UnityEngine;

namespace _Scripts.Player
{
    public class WalkingState : PlayerMovementState
    {
        protected override bool CanBeEntered => 
            IsGrounded && Fsm.IsMovingInputActive && 
            !Fsm.IsCrouchingInputActive && 
            !Fsm.IsRunningInputActive;

        public WalkingState(PlayerMovementStateMachine fsm) : base(fsm)
        {
        }

        public override void EnterState()
        {
            Fsm.SetCurrentSpeed(Stats.WalkSpeed);
        }

        public override void UpdateState()
        {
            base.UpdateState();
        
            CharacterController.Move(Fsm.SetDirection() * (Fsm.CurrentSpeed * Time.deltaTime));
        }
    }
}