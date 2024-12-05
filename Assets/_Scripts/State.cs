using System.Linq;

public abstract class State
{
    protected virtual bool CanBeEntered => true;
    protected virtual bool CanBeEnded => true;
    
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

    private bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        if (!CanBeEnded) return false;

        stateToSwitch = _fsm.States.FirstOrDefault(x => x.CanBeEntered && x != this);
        
        return stateToSwitch != null;
    }
}
