using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerEvents))]
public abstract class PlayerStateMachine : StateMachine
{
    protected PlayerInputs.PlayerActions PlayerActions { get; private set; }
    
    protected Animator Animator { get; private set; }
    public PlayerEvents PlayerEvents { get; private set; }
    protected PlayerAnimatorController PlayerAnimatorController { get; private set; }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        PlayerEvents = GetComponent<PlayerEvents>();
        PlayerAnimatorController = GetComponent<PlayerAnimatorController>();
        
        PlayerActions = InputsManager.Instance.PlayerActions;
    }
    
}
