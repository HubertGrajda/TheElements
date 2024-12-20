using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.AI
{
    public abstract class AIState : State
    {
        protected NavMeshAgent Agent { get; }
        protected Animator Animator { get; }
        protected AIStateMachine Fsm { get; }
        protected AIStatsConfig Stats { get; }
    
        protected Transform TargetTransform => Fsm.PlayerTransform;
        protected bool HasTarget => TargetTransform != null;
        protected float DistanceToTarget => Vector3.Distance(Fsm.transform.position, TargetTransform.position);

        protected AIState(AIStateMachine fsm) : base(fsm)
        {
            Fsm = fsm;
            Agent = Fsm.Agent;
            Animator = Fsm.Anim;
            Stats = Fsm.Stats;
        }
    }
}