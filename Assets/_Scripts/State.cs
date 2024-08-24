
public abstract class State
{
    protected State(StateMachine fsm)
    {
        _fsm = fsm;
    }
    
    private readonly StateMachine _fsm;
    
    public virtual void EnterState()
    {
    }

    public virtual void UpdateState()
    {
        if (TryGetStateToSwitch(out var stateToSwitch))
        {
            _fsm.ChangeState(stateToSwitch);
        }
    }

    public virtual void EndState()
    {
    }

    protected abstract bool TryGetStateToSwitch(out State stateToSwitch);
}
