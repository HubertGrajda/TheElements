using System.Collections.Generic;
using _Scripts.Spells;
using UnityEngine.InputSystem;
using _Scripts.Managers;

namespace _Scripts.Player
{
    public class BendingState : State
    {
        public SpellConfig ActiveSpell => _spells != null && _spells.Count > _currSpellIndex 
            ? _spells[_currSpellIndex] 
            : null;
    
        private readonly PlayerInputs.PlayerActions _playerActions;
        private readonly SpellsManager _spellsManager;
        private readonly ElementType _elementType;
    
        private List<SpellConfig> _spells;
        private int _currSpellIndex;
    
        protected override bool CanBeEntered => false;
    
        public BendingState(PlayerBendingStateMachine fsm, ElementType type) : base(fsm)
        {
            _elementType = type;
            _spellsManager = SpellsManager.Instance;
            _playerActions = InputsManager.Instance.PlayerActions;
            
            _spellsManager.OnSpellSelected += _ => RefreshSelectedSpells();
            _spellsManager.OnSpellDeselected += _ => RefreshSelectedSpells();
        }

        private void RefreshSelectedSpells()
        {
            _spellsManager.SelectedSpells.TryGetValue(_elementType, out _spells);
            _spellsManager.OnActiveSpellChanged?.Invoke(_elementType, ActiveSpell);
        }
        
        public override void EnterState()
        {
            RefreshSelectedSpells();
            AddListeners();
            
            _spellsManager.OnActiveSpellChanged?.Invoke(_elementType, ActiveSpell);
        }

        private void NextSpell(InputAction.CallbackContext _)
        {
            if (_spells == null) return;
            if (_currSpellIndex >= _spells.Count - 1) return;
        
            _currSpellIndex++;
            _spellsManager.OnActiveSpellChanged(_elementType, ActiveSpell);
        }

        private void PreviousSpell(InputAction.CallbackContext _)
        {
            if (_spells == null) return;
            if (_currSpellIndex <= 0) return;
        
            _currSpellIndex--;
            _spellsManager.OnActiveSpellChanged(_elementType, ActiveSpell);
        }

        private void AddListeners()
        {
            _playerActions.NextSpell.started += NextSpell;
            _playerActions.PreviousSpell.started += PreviousSpell;
        }

        private void RemoveListeners()
        {
            _playerActions.NextSpell.started -= NextSpell;
            _playerActions.PreviousSpell.started -= PreviousSpell;
        }
        
        public override void EndState()
        {
            RemoveListeners(); 
        }
    }
}