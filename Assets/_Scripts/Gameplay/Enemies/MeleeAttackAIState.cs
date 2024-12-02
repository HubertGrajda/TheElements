using UnityEngine;

public class MeleeAttackAIState : AIState
{
    public MeleeAttackAIState(AIStateMachine fsm) : base(fsm)
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
        
        if (Fsm.DistanceToTarget > Fsm.Stats.MeleeAttackRange)
        {
            stateToSwitch = Fsm.RangedAttackState;
        }

        return stateToSwitch != null;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_timer < 0)
        {
            Animator.SetTrigger(MeleeAttack);
            _timer = Fsm.Stats.MeleeAttackCooldown;
        }
        
        _timer -= Time.deltaTime;
    }
}
