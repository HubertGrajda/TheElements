using _Scripts.Managers;
using _Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

namespace _Scripts.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIStateMachine : StateMachine
    {
        [field: SerializeField] public AIStatsConfig Stats { get; private set; }
    
        public AISpellLauncher SpellLauncher { get; private set; }
        public Transform PlayerTransform { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Animator Anim { get; private set; }

        private HealthSystem _healthSystem;
        
        protected void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Anim = GetComponent<Animator>();
            SpellLauncher = GetComponent<AISpellLauncher>();
        
            AddListeners();
        }

        protected override void InitStates(out State entryState)
        {
            States.Add(new IdleAIState(this));
            States.Add(new MovementAIState(this));
            States.Add(new MeleeAttackAIState(this));
            States.Add(new SpellAttackAIState(this));
        
            entryState = States[0];
        }

        protected override void Start()
        {
            base.Start();

            if (PlayerManager.Instance.TryGetPlayerComponent(out PlayerController controller))
            {
                PlayerTransform = controller.transform;
            }
        }

        private void OnDestroy() => RemoveListeners();

        private void AddListeners()
        {
            if (TryGetComponent(out _healthSystem))
            {
                _healthSystem.OnDeath += OnDeath;
                _healthSystem.OnDamaged += OnDamaged;
            }
        }

        private void RemoveListeners()
        {
            if (_healthSystem != null)
            {
                _healthSystem.OnDamaged -= OnDamaged;
                _healthSystem.OnDeath -= OnDeath;
            }
        }
    
        private void OnDamaged(int _)
        {
            Anim.SetTrigger(Constants.AnimationNames.DAMAGED);
        }
    
        private void OnDeath()
        {
            Anim.SetTrigger(Constants.AnimationNames.DEATH);
            Agent.isStopped = true;
            StopAllCoroutines();
            enabled = false;
        }
    
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var center = transform.position;
        
            Handles.color = new Color(1, 0, 0, 0.03f);
            Handles.DrawSolidDisc(center, Vector3.up, Stats.MeleeAttackRange);
        
            Handles.color = new Color(0, 1, 1, 0.02f);
            Handles.DrawSolidDisc(center, Vector3.up, Stats.RangedAttackMinDistance);
        
            Handles.color = new Color(0, 1, 0, 0.01f);
            Handles.DrawSolidDisc(center, Vector3.up, Stats.RangedAttackMaxDistance);

            Handles.color = new Color(0, 0, 1, 0.005f);
            Handles.DrawSolidDisc(center, Vector3.up, Stats.FollowingTargetRange);
        }
#endif

    }
}