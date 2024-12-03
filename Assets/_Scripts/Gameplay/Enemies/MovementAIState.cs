using System.Collections;
using UnityEngine;

public class MovementAIState : AIState
{
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
        if (Fsm.PlayerTransform == null) return;
        
        base.UpdateState();
        
        Agent.speed = Fsm.Stats.MovementSpeed * _currentSpeed;
        
        Agent.destination = Fsm.PlayerTransform.position;
    }
    public override void EndState()
    {
        _decreaseSpeedCoroutine = Fsm.StartCoroutine(StopMoving());
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (Fsm.DistanceToTarget < Fsm.Stats.RangedAttackRange)
        {
            stateToSwitch = Fsm.RangedAttackState;
            
        }
        else if (Fsm.DistanceToTarget > Fsm.Stats.FollowingTargetRange)
        {
            stateToSwitch = Fsm.IdleState;
        }

        return stateToSwitch != null;
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

