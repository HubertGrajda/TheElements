using System.Linq;

namespace _Scripts.AI
{
    public class IdleAIState : AIState
    {
        protected override bool CanBeEntered => !HasTarget || DistanceToTarget > Stats.TriggeredDetectionRange;

        private readonly Detector _detector;
        
        public IdleAIState(AIStateMachine fsm) : base(fsm)
        {
            _detector = Fsm.GetComponent<Detector>();
            _detector.ChangeDetectionRange(Stats.FollowingTargetRange);
        }
        
        public override void EnterState()
        {
            base.EnterState();
            _detector.ChangeDetectionRange(Stats.FollowingTargetRange);
            HealthSystem.OnDamaged += OnDamaged;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            if (HasTarget) return;
            
            DetectTarget();
        }

        private void DetectTarget()
        {
            if (!_detector.TryGetDetected(out var detectedTarget)) return;
            
            Fsm.ChangeTarget(detectedTarget.First().transform);
        }

        public override void EndState()
        {
            base.EndState();
            HealthSystem.OnDamaged -= OnDamaged;
        }

        private void OnDamaged(int obj)
        {
            _detector.ChangeDetectionRange(Stats.TriggeredDetectionRange);
        }
    }
}