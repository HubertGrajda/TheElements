using UnityEngine;

public class RangedAttackAIState : AIState
{
    protected override bool CanBeEntered =>
        HasTarget &&
        DistanceToTarget > Stats.RangedAttackMinDistance &&
        DistanceToTarget < Stats.RangedAttackMaxDistance;

    protected override bool CanBeEnded => !CanBeEntered && !IsCastingRangeAttack;

    private bool IsCastingRangeAttack => Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == RangeAttack;
    
    public RangedAttackAIState(AIStateMachine fsm) : base(fsm) { }
    
    private float _timer;
    
    private static readonly int RangeAttack = Animator.StringToHash("RangeAttack");

    public override void UpdateState()
    {
        base.UpdateState();
        
        Fsm.transform.LookAt(TargetTransform);
        
        if (_timer < 0)
        {
            Animator.SetTrigger(RangeAttack);
            _timer = Stats.ShootingFrequency;
        }
        
        _timer -= Time.deltaTime;
    }
}
