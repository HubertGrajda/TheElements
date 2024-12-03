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

    private AIHealthSystem _healthSystem;
    private BaseHealthSystem _playerHealthSystem;
    
    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        AddListeners();
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

        if (PlayerManager.Instance.TryGetPlayerComponent(out PlayerController controller))
        {
            PlayerTransform = controller.transform;
        }
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (PlayerTransform == null) return;
        
        DistanceToTarget = Vector3.Distance(transform.position, PlayerTransform.position);
    }

    private void OnDestroy() => RemoveListeners();

    private void AddListeners()
    {
        if (TryGetComponent(out _healthSystem))
        {
            _healthSystem.OnDeath += OnDeath;
        }

        if (PlayerManager.Instance.TryGetComponent(out _playerHealthSystem))
        {
            _playerHealthSystem.OnDeath += OnPlayerDeath;
        }
    }
    
    private void RemoveListeners()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDeath -= OnDeath;
        }

        if (_playerHealthSystem != null)
        {
            _playerHealthSystem.OnDeath -= OnPlayerDeath;
        }
    }
    
    private void OnDeath()
    {
        enabled = false;
        StopAllCoroutines();
    }
    
    private void OnPlayerDeath()
    {
        ChangeState(IdleState);
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
