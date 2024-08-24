using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine.InputSystem;

public abstract class BendingState : State
{
    protected readonly List<Spell> spells = new ();
    private int _currSpellIndex;

    public Spell SelectedSpell => spells[_currSpellIndex];

    protected BendingState(PlayerBendingStateMachine fsm) : base(fsm)
    {
    }

    public override void EnterState()
    {
        Managers.InputManager.PlayerActions.NextSpell.started += NextSpell;
        Managers.InputManager.PlayerActions.PreviousSpell.started += PreviousSpell;
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
        Managers.InputManager.PlayerActions.NextSpell.started -= NextSpell;
        Managers.InputManager.PlayerActions.PreviousSpell.started -= PreviousSpell;
    }
}
