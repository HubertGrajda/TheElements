using System.Collections;
using UnityEngine;

namespace _Scripts.AI
{
    public class MovementAIState : AIState
    {
        protected override bool CanBeEntered => HasTarget && DistanceToTarget < Stats.FollowingTargetRange;

        public MovementAIState(AIStateMachine fsm) : base(fsm)
        {
        }
    
        private float _currentSpeed;
        private Coroutine _increaseSpeedCoroutine;
        private Coroutine _decreaseSpeedCoroutine;
    
    
        public override void EnterState()
        {
            _increaseSpeedCoroutine = Fsm.StartCoroutine(StartMoving());
        }

        public override void UpdateState()
        {
            base.UpdateState();
        
            Agent.speed = Stats.MovementSpeed * _currentSpeed;
            Agent.destination = TargetTransform.position;
        }
        public override void EndState()
        {
            _decreaseSpeedCoroutine = Fsm.StartCoroutine(StopMoving());
        }

        private IEnumerator StopMoving()
        {
            if(_increaseSpeedCoroutine != null)
                Fsm.StopCoroutine(_increaseSpeedCoroutine);
        
            _currentSpeed = 1f;
            while (_currentSpeed > 0)
            {
                _currentSpeed -= 0.1f;
                Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, _currentSpeed);
                yield return null;
            }
        
            Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 0f);
            Agent.isStopped = true;
            _decreaseSpeedCoroutine = null;
        }

        private IEnumerator StartMoving()
        {
            if (_decreaseSpeedCoroutine != null)
            {
                Fsm.StopCoroutine(_decreaseSpeedCoroutine);
            }

            Agent.isStopped = false;
            _currentSpeed = 0f;
        
            while (_currentSpeed < 1f)
            {
                _currentSpeed += 0.01f;
                Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, _currentSpeed);
                yield return null;
            }
        
            Animator.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 1f);
            _increaseSpeedCoroutine = null;
        }
    }
}