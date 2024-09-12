using System.Collections.Generic;
using _Scripts.Spells;
using UnityEngine.InputSystem;

public abstract class BendingState : State
{
    protected readonly List<Spell> spells = new ();
    private int _currSpellIndex;

    public Spell SelectedSpell => spells[_currSpellIndex];

    private PlayerInputs.PlayerActionsActions _playerActions;
    
    protected BendingState(PlayerBendingStateMachine fsm) : base(fsm)
    {
        _playerActions = InputManager.Instance.PlayerActions;
    }

    public override void EnterState()
    {
        _playerActions.NextSpell.started += NextSpell;
        _playerActions.PreviousSpell.started += PreviousSpell;
    }

    private void NextSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex >= spells.Count - 1) return;
        
        _currSpellIndex++;
    }

    private void PreviousSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex <= 0) return;
        
        _currSpellIndex--;
    }

    public override void EndState()
    {
        _playerActions.NextSpell.started -= NextSpell;
        _playerActions.PreviousSpell.started -= PreviousSpell;
    }
}
