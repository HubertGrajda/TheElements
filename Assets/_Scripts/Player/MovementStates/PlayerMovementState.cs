using UnityEngine;

namespace _Scripts.Player
{
    public abstract class PlayerMovementState : State
    {
        protected PlayerMovementStateMachine Fsm { get;}
        protected PlayerMovementStatsConfig Stats { get;}
        protected CharacterController CharacterController { get;}

        protected bool IsGrounded => Fsm.GroundDetector.IsGrounded;
    
        protected PlayerMovementState(PlayerMovementStateMachine fsm) : base(fsm)
        {
            Fsm = fsm;
            Stats = Fsm.MovementStats;
            CharacterController = fsm.CharacterController;
        }
    }
}