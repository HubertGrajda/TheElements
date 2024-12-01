using System.Collections.Generic;
using _Scripts.Spells;
using UnityEngine.InputSystem;
using _Scripts.Managers;

public class BendingState : State
{
    private readonly List<Spell> _spells = new();
    private int _currSpellIndex;

    public Spell SelectedSpell => _spells[_currSpellIndex];

    private PlayerInputs.PlayerActions _playerActions;
    
    public BendingState(PlayerBendingStateMachine fsm, ElementType type) : base(fsm)
    {
        foreach (var spell in fsm.Spells)
        {
            if (spell.ElementType != type) continue;
            
            _spells.Add(spell.SpellPrefab);
        }
        
        SpellsManager.Instance.OnSelectedSpellChanged?.Invoke(SelectedSpell);
        _playerActions = InputsManager.Instance.PlayerActions;
    }

    public override void EnterState()
    {
        _playerActions.NextSpell.started += NextSpell;
        _playerActions.PreviousSpell.started += PreviousSpell;
    }

    private void NextSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex >= _spells.Count - 1) return;
        
        _currSpellIndex++;
        SpellsManager.Instance.OnSelectedSpellChanged(SelectedSpell);
    }

    private void PreviousSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex <= 0) return;
        
        _currSpellIndex--;
        SpellsManager.Instance.OnSelectedSpellChanged(SelectedSpell);
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
