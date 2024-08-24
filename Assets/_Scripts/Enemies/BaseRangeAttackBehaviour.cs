using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(AIStateMachine))]
public abstract class BaseRangeAttackBehaviour : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPoint;
    
    protected Transform target;
    protected AIBaseStats_SO stats;
    
    protected void Start()
    {
        target = Managers.PlayerManager.PlayerRef.transform;
        stats = GetComponent<AIStateMachine>().stats;
    }

    public abstract void OnLaunchRangeAttack();
    public abstract void OnCastRangeAttack();
    
}
