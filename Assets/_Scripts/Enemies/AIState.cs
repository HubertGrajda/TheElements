using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : State
{
    protected readonly NavMeshAgent agent;
    protected readonly Animator anim;
    protected readonly Transform target;

    protected AIStateMachine _fsm;
    
    protected AIState(AIStateMachine fsm) : base(fsm)
    {
        _fsm = fsm;
        target = _fsm.PlayerTransform;
        agent = _fsm.Agent;
        anim = _fsm.Anim;
    }
}
