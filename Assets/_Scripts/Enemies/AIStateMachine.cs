using _Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIStateMachine : StateMachine
{
    public Transform PlayerTransform { get; private set; }

    public AIBaseStats_SO stats;
    public float DistanceToTarget { get; private set; }

    public AIState_Idle IdleState { get; private set; }
    public AIState_Moving MovingState { get; private set; }
    public AIState_MeleeAttack MeleeAttackState { get; private set; }
    public AIState_RangedAttack RangedAttackState { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }

    private BaseRangeAttackBehaviour rangeAttackBehaviour;
    
    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        rangeAttackBehaviour = GetComponent<BaseRangeAttackBehaviour>();
    }
    
    protected override void InitStates(out State entryState)
    {
        IdleState = new AIState_Idle(this);
        MovingState = new AIState_Moving(this);
        MeleeAttackState =  new AIState_MeleeAttack(this);
        RangedAttackState = new AIState_RangedAttack(this, rangeAttackBehaviour);
        
        entryState = IdleState;
    }

    protected override void Start()
    {
        base.Start();
        
        PlayerTransform = Managers.PlayerManager.PlayerRef.transform;
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (PlayerTransform == null) return;
        DistanceToTarget = Vector3.Distance(transform.position, PlayerTransform.position);

        if (CurrentState != IdleState && CurrentState != MovingState)
        {
            transform.LookAt(PlayerTransform);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var center = transform.position;
        
        Handles.color = new Color(1, 0, 0, 0.03f);
        Handles.DrawSolidDisc(center, Vector3.up, stats.meleeAttackRange);
        
        Handles.color = new Color(0, 1, 0, 0.01f);
        Handles.DrawSolidDisc(center, Vector3.up, stats.rangedAttackRange);

        Handles.color = new Color(0, 0, 1, 0.005f);
        Handles.DrawSolidDisc(center, Vector3.up, stats.followingTargetRange);
    }
#endif

}
