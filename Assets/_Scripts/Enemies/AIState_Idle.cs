public class AIState_Idle : AIState
{
    public AIState_Idle(AIStateMachine fsm) : base(fsm)
    {
    }

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
            
        if (_fsm.DistanceToTarget < _fsm.stats.followingTargetRange)
        {
            stateToSwitch = _fsm.MovingState;
        }

        return stateToSwitch != null;
    }
}
