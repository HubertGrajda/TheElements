using System.Collections.Generic;
using _Scripts.Spells;
using UnityEngine.InputSystem;
using _Scripts.Managers;

public class BendingState : State
{
    protected readonly List<Spell> spells = new ();
    private int _currSpellIndex;

    public Spell SelectedSpell => spells[_currSpellIndex];

    private PlayerInputs.PlayerActions _playerActions;
    
    public BendingState(PlayerBendingStateMachine fsm, ElementType type) : base(fsm)
    {
        foreach (var spell in fsm.Spells)
        {
            if (spell.ElementType != type) continue;
            
            spells.Add(spell.SpellPrefab);
        }
        
        _playerActions = InputsManager.Instance.PlayerActions;
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

    protected override bool TryGetStateToSwitch(out State stateToSwitch)
    {
        stateToSwitch = default;
        
        return false;
    }
}
