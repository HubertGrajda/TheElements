using _Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

namespace _Scripts.AI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AISpellLauncher))]
    [RequireComponent(typeof(HealthSystem))]
    public class AIStateMachine : StateMachine
    {
        [field: SerializeField] public AIStatsConfig Stats { get; private set; }
    
        public AISpellLauncher SpellLauncher { get; private set; }
        public HealthSystem HealthSystem { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Animator Anim { get; private set; }
        public Transform TargetTransform { get; private set; }
        
        protected void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Anim = GetComponent<Animator>();
            SpellLauncher = GetComponent<AISpellLauncher>();
            HealthSystem = GetComponent<HealthSystem>();
        
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

        public void ChangeTarget(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }

        private void OnDestroy() => RemoveListeners();

        private void AddListeners()
        {
            HealthSystem.OnDeath += OnDeath;
            HealthSystem.OnDamaged += OnDamaged;
        }

        private void RemoveListeners()
        {
            HealthSystem.OnDamaged -= OnDamaged;
            HealthSystem.OnDeath -= OnDeath;
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