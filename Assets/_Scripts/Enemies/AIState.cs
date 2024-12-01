using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : State
{
    protected  NavMeshAgent Agent { get; }
    protected  Animator Animator { get; }
    protected AIStateMachine Fsm { get; }

    protected AIState(AIStateMachine fsm) : base(fsm)
    {
        Fsm = fsm;
        Agent = Fsm.Agent;
        Animator = Fsm.Anim;
    }
}
