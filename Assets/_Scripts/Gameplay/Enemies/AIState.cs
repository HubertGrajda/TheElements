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
        protected HealthSystem HealthSystem { get; }
        
        protected bool HasTarget => Fsm.TargetTransform != null;

        protected float DistanceToTarget => HasTarget
            ? Vector3.Distance(Fsm.transform.position, Fsm.TargetTransform.position)
            : Mathf.Infinity;

        protected AIState(AIStateMachine fsm) : base(fsm)
        {
            Fsm = fsm;
            Agent = Fsm.Agent;
            Animator = Fsm.Anim;
            Stats = Fsm.Stats;
            HealthSystem = Fsm.HealthSystem;
        }
    }
}