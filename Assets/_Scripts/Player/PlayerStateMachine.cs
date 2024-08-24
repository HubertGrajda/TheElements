using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerEvents))]
public abstract class PlayerStateMachine : StateMachine
{
    protected PlayerInputs.PlayerActionsActions playerActions;
    
    protected Animator anim;
    public Animator Anim => anim;

    public PlayerEvents PlayerEvents { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        PlayerEvents = GetComponent<PlayerEvents>();
        playerActions = Managers.InputManager.Inputs.PlayerActions;
    }
    
}
