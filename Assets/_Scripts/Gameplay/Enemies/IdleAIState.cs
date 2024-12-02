public class IdleAIState : AIState
{
    public IdleAIState(AIStateMachine fsm) : base(fsm)
    {
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
            
        if (Fsm.DistanceToTarget < Fsm.Stats.FollowingTargetRange)
        {
            stateToSwitch = Fsm.MovingState;
        }

        return stateToSwitch != null;
    }
}
