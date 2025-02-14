using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;

namespace _Scripts.UI
{
    public class SpellsSelector : MonoBehaviour
    {
        [SerializeField] private ElementType elementType;
        [SerializeField] private List<SelectedSpellSlot> slots;
        
        private List<SpellConfig> _selectedSpells = new();
        
        private SpellsManager _spellsManager;

        private void Awake()
        {
            _spellsManager = SpellsManager.Instance;

            Prepare();
            AddListeners();
        }

        private void AddListeners()
        {
            _spellsManager.OnSpellSelected += OnSpellSelected;
            _spellsManager.OnSpellDeselected += OnSpellDeselected;
        }

        private void OnSpellDeselected(SpellConfig spell)
        {
            if (spell.ElementType != elementType) return;
            if (!TryGetSlotWithSpell(spell, out var slot)) return;

            slot.UnassignSpell();
        }

        private void OnSpellSelected(SpellConfig spell)
        {
            if (spell.ElementType != elementType) return;
            if (!TryGetEmptySlot(out var slot)) return;
            
            slot.AssignSpell(spell);
        }

        private void Prepare()
        {
            if (!_spellsManager.SelectedSpells.TryGetValue(elementType, out _selectedSpells)) return;

            if (_selectedSpells.Count > slots.Count) return;
            
            for (var i = 0; i < _selectedSpells.Count; i++)
            {
                slots[i].AssignSpell(_selectedSpells[i]);
            } 
        }

        private bool TryGetEmptySlot(out SelectedSpellSlot slot) => 
            slot = slots.FirstOrDefault(x => x.Spell == null);

        private bool TryGetSlotWithSpell(SpellConfig spell, out SelectedSpellSlot slot) =>
            slot = slots.FirstOrDefault(x => x.Spell == spell);
    }
}