using UnityEngine;

namespace _Scripts.AI
{
    public class MeleeAttackAIState : AIState
    {
        protected override bool CanBeEntered => HasTarget && DistanceToTarget < Stats.MeleeAttackRange;
        protected override bool CanBeEnded => !CanBeEntered && DistanceToTarget > Stats.MeleeAttackRange;
    
        private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
        private float _timer;
    
        public MeleeAttackAIState(AIStateMachine fsm) : base(fsm)
        {
        }

        public override void UpdateState()
        {
            base.UpdateState();
        
            Fsm.transform.LookAt(TargetTransform);
        
            if (_timer < 0)
            {
                Animator.SetTrigger(MeleeAttack);
                _timer = Stats.MeleeAttackCooldown;
            }
        
            _timer -= Time.deltaTime;
        }
    }
}