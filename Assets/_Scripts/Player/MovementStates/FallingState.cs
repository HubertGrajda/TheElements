
public class FallingState : State
{
    public FallingState(StateMachine fsm) : base(fsm)
    {
    }
    
    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        throw new System.NotImplementedException();
    }
}
