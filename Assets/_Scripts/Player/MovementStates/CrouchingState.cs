using UnityEngine;

namespace _Scripts.Player
{
    public class CrouchingState : PlayerMovementState
    {
        protected override bool CanBeEntered => IsGrounded && Fsm.IsCrouchingInputActive;

        public CrouchingState(PlayerMovementStateMachine fsm) : base(fsm)
        {
        }
    
        public override void EnterState()
        {
            Fsm.OnCrouchingStateChanged?.Invoke(true);
        }

        public override void UpdateState()
        {
            base.UpdateState();
        
            var speed = Fsm.IsMovingInputActive ? Stats.CrouchSpeed : 0f;
            Fsm.SetCurrentSpeed(speed);
        
            CharacterController.Move(Fsm.SetDirection() * (Fsm.CurrentSpeed * Time.deltaTime));
        }

        public override void EndState()
        {
            Fsm.OnCrouchingStateChanged?.Invoke(false);
        }
    }
}