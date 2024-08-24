using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State CurrentState { get; private set; }

    protected virtual void Start()
    {
        InitStates(out var state);
        CurrentState = state;
    }

    protected virtual void Update()
    {
        CurrentState?.UpdateState();
    }

    public void ChangeState(State nextState)
    {
        CurrentState.EndState();
        CurrentState = nextState;
        CurrentState.EnterState();
    }

    protected abstract void InitStates(out State entryState);
}
