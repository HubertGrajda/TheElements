using UnityEngine;

public class RangedAttackAIState : AIState
{
    public RangedAttackAIState(AIStateMachine fsm) : base(fsm) { }
    
    private float _timer;
    private bool _isCastingRangeAttack;
    
    private static readonly int RangeAttack = Animator.StringToHash("RangeAttack");

    public override void UpdateState()
    {
        if (Fsm.PlayerTransform == null) return;
        Fsm.transform.LookAt(Fsm.PlayerTransform);
        
        base.UpdateState();
        
        if (_timer < 0)
        {
            Animator.SetTrigger(RangeAttack);
            _timer = Fsm.Stats.ShootingFrequency;
        }
        
        _timer -= Time.deltaTime;
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        if (_isCastingRangeAttack) return false;
        
        if (Fsm.DistanceToTarget < Fsm.Stats.MeleeAttackRange)
        {
            stateToSwitch = Fsm.MeleeAttackState;
        }
        else if (Fsm.DistanceToTarget > Fsm.Stats.RangedAttackRange)
        {
            stateToSwitch = Fsm.MovingState;
        }

        return stateToSwitch != null;
    }

    public void BlockStateSwitching(bool block) => _isCastingRangeAttack = block; 
    
}
