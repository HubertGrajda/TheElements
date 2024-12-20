using System.Collections;
using UnityEngine;

namespace _Scripts.Player
{
    public class RunningState : PlayerMovementState
    {
        protected override bool CanBeEntered =>
            IsGrounded &&
            Fsm.IsMovingInputActive &&
            Fsm.IsRunningInputActive;

        private Coroutine _increaseSpeedCoroutine;
    
        public RunningState(PlayerMovementStateMachine fsm) : base(fsm)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            _increaseSpeedCoroutine = Fsm.StartCoroutine(IncreaseSpeedOverTime());
        }

        public override void UpdateState()
        {
            CharacterController.Move(Fsm.SetDirection() * (Fsm.CurrentSpeed * Time.deltaTime));
        
            base.UpdateState();
        }

        public override void EndState()
        {
            if (_increaseSpeedCoroutine != null)
            {
                Fsm.StopCoroutine(_increaseSpeedCoroutine);
            }
        
            base.EndState();
        }

        private IEnumerator IncreaseSpeedOverTime()
        {
            var runningTime = 0f;
        
            while (runningTime <= Stats.RunAccelerationTime)
            {
                Fsm.SetCurrentSpeed(Mathf.Lerp(Stats.RunSpeed, Stats.RunMaxSpeed, Mathf.Clamp01(runningTime / Stats.RunAccelerationTime)));
                runningTime += Time.deltaTime;
                yield return null;
            }

            Fsm.SetCurrentSpeed(Stats.RunMaxSpeed);
            _increaseSpeedCoroutine = null;
        }
    }
}