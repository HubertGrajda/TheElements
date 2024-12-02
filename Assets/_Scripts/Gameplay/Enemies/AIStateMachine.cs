using _Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIStateMachine : StateMachine
{
    [field: SerializeField] public AIStatsConfig Stats { get; private set; }
    
    public Transform PlayerTransform { get; private set; }
    public float DistanceToTarget { get; private set; }
    
    public IdleAIState IdleState { get; private set; }
    public MovementAIState MovingState { get; private set; }
    public MeleeAttackAIState MeleeAttackState { get; private set; }
    public RangedAttackAIState RangedAttackState { get; private set; }
    
    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }
    
    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
    }
    
    protected override void InitStates(out State entryState)
    {
        IdleState = new IdleAIState(this);
        MovingState = new MovementAIState(this);
        MeleeAttackState =  new MeleeAttackAIState(this);
        RangedAttackState = new RangedAttackAIState(this);
        
        entryState = IdleState;
    }

    protected override void Start()
    {
        base.Start();
        
        PlayerTransform = PlayerManager.Instance.PlayerRef.transform;
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
        Handles.DrawSolidDisc(center, Vector3.up, Stats.MeleeAttackRange);
        
        Handles.color = new Color(0, 1, 0, 0.01f);
        Handles.DrawSolidDisc(center, Vector3.up, Stats.RangedAttackRange);

        Handles.color = new Color(0, 0, 1, 0.005f);
        Handles.DrawSolidDisc(center, Vector3.up, Stats.FollowingTargetRange);
    }
#endif

}
