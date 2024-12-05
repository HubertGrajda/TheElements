using System.Collections.Generic;
using _Scripts.Spells;
using UnityEngine.InputSystem;
using _Scripts.Managers;

public class BendingState : State
{
    public Spell SelectedSpell => _spells[_currSpellIndex];

    private readonly List<Spell> _spells = new();
    private int _currSpellIndex;

    private readonly PlayerInputs.PlayerActions _playerActions;
    private readonly SpellsManager _spellsManager;
    
    protected override bool CanBeEntered => false;
    
    public BendingState(PlayerBendingStateMachine fsm, ElementType type) : base(fsm)
    {
        foreach (var spell in fsm.Spells)
        {
            if (spell.ElementType != type) continue;
            
            _spells.Add(spell.SpellPrefab);
        }

        _spellsManager = SpellsManager.Instance;
        _playerActions = InputsManager.Instance.PlayerActions;
    }

    public override void EnterState()
    {
        _playerActions.NextSpell.started += NextSpell;
        _playerActions.PreviousSpell.started += PreviousSpell;
        _spellsManager.OnSelectedSpellChanged?.Invoke(SelectedSpell);
    }

    private void NextSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex >= _spells.Count - 1) return;
        
        _currSpellIndex++;
        _spellsManager.OnSelectedSpellChanged(SelectedSpell);
    }

    private void PreviousSpell(InputAction.CallbackContext context)
    {
        if (_currSpellIndex <= 0) return;
        
        _currSpellIndex--;
        _spellsManager.OnSelectedSpellChanged(SelectedSpell);
    }

    public override void EndState()
    {
        _playerActions.NextSpell.started -= NextSpell;
        _playerActions.PreviousSpell.started -= PreviousSpell;
    }
}
