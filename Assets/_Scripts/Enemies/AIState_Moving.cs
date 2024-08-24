using System.Collections;
using UnityEngine;

public class AIState_Moving : AIState
{
    public AIState_Moving(AIStateMachine fsm) : base(fsm)
    {
    }
    
    private float _currentSpeed;
    private Coroutine _increaseSpeedCoroutine;
    private Coroutine _decreaseSpeedCoroutine;
    
    public override void EnterState()
    {
        _increaseSpeedCoroutine = _fsm.StartCoroutine(StartMoving());
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        agent.speed = _fsm.stats.movementSpeed * _currentSpeed;
        agent.destination = _fsm.PlayerTransform.position;
    }
    public override void EndState()
    {
        _decreaseSpeedCoroutine = _fsm.StartCoroutine(StopMoving());
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.DistanceToTarget < _fsm.stats.rangedAttackRange)
        {
            stateToSwitch = _fsm.RangedAttackState;
            
        }
        else if (_fsm.DistanceToTarget > _fsm.stats.followingTargetRange)
        {
            stateToSwitch = _fsm.IdleState;
        }

        return stateToSwitch != null;
    }

    private IEnumerator StopMoving()
    {
        if(_increaseSpeedCoroutine != null)
            _fsm.StopCoroutine(_increaseSpeedCoroutine);
        
        _currentSpeed = 1f;
        while (_currentSpeed > 0)
        {
            _currentSpeed -= 0.1f;
            anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, _currentSpeed);
            yield return null;
        }
        
        anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 0f);
        agent.isStopped = true;
        _decreaseSpeedCoroutine = null;
    }

    private IEnumerator StartMoving()
    {
        if(_decreaseSpeedCoroutine != null)
            _fsm.StopCoroutine(_decreaseSpeedCoroutine);

        agent.isStopped = false;
        _currentSpeed = 0f;
        while (_currentSpeed < 1f)
        {
            _currentSpeed += 0.01f;
            anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, _currentSpeed);
            yield return null;
        }
        
        anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, 1f);
        _increaseSpeedCoroutine = null;
    }
}

