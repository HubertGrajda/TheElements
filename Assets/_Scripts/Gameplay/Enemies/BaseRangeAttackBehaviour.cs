using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(AIStateMachine))]
public abstract class BaseRangeAttackBehaviour : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPoint;
    
    protected Transform Target { get; private set; }
    protected AIStatsConfig Stats { get; private set; }

    protected void Start()
    {
        if (PlayerManager.Instance.TryGetPlayerController(out var controller))
        {
            Target = controller.transform;
        }
        
        Stats = GetComponent<AIStateMachine>().Stats;
    }

    public abstract void OnLaunchRangeAttack();

    public virtual void OnCastRangeAttack()
    {
    }
    
}
