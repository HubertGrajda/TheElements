using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerEvents))]
public abstract class PlayerStateMachine : StateMachine
{
    protected PlayerInputs.PlayerActions PlayerActions { get; private set; }
    
    public Animator Animator { get; private set; }
    public PlayerEvents PlayerEvents { get; private set; }
    protected PlayerManager PlayerManager { get; private set; }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        PlayerEvents = GetComponent<PlayerEvents>();
        PlayerActions = InputsManager.Instance.PlayerActions;
        PlayerManager = PlayerManager.Instance;
    }
    
}
