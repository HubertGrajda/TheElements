using UnityEngine;

public class AIState_RangedAttack : AIState
{
    public AIState_RangedAttack(AIStateMachine fsm, BaseRangeAttackBehaviour attackBehaviour) : base(fsm) { }
    
    private float _timer;
    private bool _isCastingRangeAttack;
    
    private static readonly int RangeAttack = Animator.StringToHash("RangeAttack");

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_timer < 0)
        {
            anim.SetTrigger(RangeAttack);
            _timer = _fsm.stats.shootingFrequency;
        }
        
        _timer -= Time.deltaTime;
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if(_isCastingRangeAttack) return false;

        if (_fsm.DistanceToTarget < _fsm.stats.meleeAttackRange)
        {
            stateToSwitch = _fsm.MeleeAttackState;
        }
        else if (_fsm.DistanceToTarget > _fsm.stats.rangedAttackRange)
        {
            stateToSwitch = _fsm.MovingState;
        }

        return stateToSwitch != null;
    }

    public void BlockStateSwitching(bool block) => _isCastingRangeAttack = block; 
    
}
