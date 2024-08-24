using UnityEngine;

public class AIState_MeleeAttack : AIState
{
    public AIState_MeleeAttack(AIStateMachine fsm) : base(fsm)
    {
    }
    
    private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
    private float _timer;
    
    public override void EndState()
    {
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_fsm.DistanceToTarget > _fsm.stats.meleeAttackRange)
        {
            stateToSwitch = _fsm.RangedAttackState;
        }

        return stateToSwitch != null;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_timer < 0)
        {
            anim.SetTrigger(MeleeAttack);
            _timer = _fsm.stats.meleeAttackCooldown;
        }
        
        _timer -= Time.deltaTime;
    }
}
