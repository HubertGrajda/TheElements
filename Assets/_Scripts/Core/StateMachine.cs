using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State CurrentState { get; private set; }
        public List<State> States { get; } = new();

        protected virtual void Start()
        {
            InitStates(out var state);
            
            if (state == null) return;
            
            ChangeState(state);
        }

        protected virtual void Update()
        {
            CurrentState?.UpdateState();
        }

        public void ChangeState(State nextState)
        {
            CurrentState?.EndState();
            CurrentState = nextState;
            CurrentState.EnterState();
        }

        protected abstract void InitStates(out State entryState);
    }
}