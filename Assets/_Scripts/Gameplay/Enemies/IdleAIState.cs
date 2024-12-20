namespace _Scripts.AI
{
    public class IdleAIState : AIState
    {
        protected override bool CanBeEntered => !HasTarget || DistanceToTarget > Stats.FollowingTargetRange;

        public IdleAIState(AIStateMachine fsm) : base(fsm)
        {
        }
    }
}