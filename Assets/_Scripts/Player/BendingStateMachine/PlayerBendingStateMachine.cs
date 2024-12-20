using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerBendingStateMachine : PlayerStateMachine
    {
        [field: SerializeField] public List<SpellConfig> Spells { get; private set; }

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private List<ElementType> bendingStyles;
    
        private readonly Dictionary<ElementType, BendingState> _elementToBendingState = new();

        private SpellsManager _spellsManager;

        private const int INITIAL_BENDING_SLOT_NUMBER = 1;
    
        public BendingState CurrentBendingState => CurrentState as BendingState;
    
        protected override void InitStates(out State entryState)
        {
            foreach (var elementType in bendingStyles)
            {
                _elementToBendingState.Add(elementType, new BendingState(this, elementType));
            }

            entryState = _elementToBendingState.First().Value;
        }

        protected override void Start()
        {
            _spellsManager = SpellsManager.Instance;
        
            base.Start();

            InitBendingStates();
            OnBendingSlotStarted(INITIAL_BENDING_SLOT_NUMBER);
            AddListeners();
        }

        private void InitBendingStates()
        {
            foreach (var bendingState in _elementToBendingState.Values)
            {
                SpellsManager.Instance.OnSelectedSpellChanged?.Invoke(bendingState.SelectedSpell);
            }
        }
    
        private void AddListeners()
        {
            PlayerActions.NumKeys.started += OnNumKeyStarted;
        }

        private void RemoveListeners()
        {
            PlayerActions.NumKeys.started -= OnNumKeyStarted;
        }
    
        private void OnNumKeyStarted(InputAction.CallbackContext context)
        {
            var value = (int)context.ReadValue<float>();
        
            OnBendingSlotStarted(value);
        }
    
        private void OnBendingSlotStarted(int slotNumber)
        {
            var slotIndex = slotNumber - 1;
        
            if (slotIndex < 0) return;
            if (bendingStyles.Count <= slotIndex) return;

            ChangeState(bendingStyles[slotIndex]);
        }

        public void ChangeState(ElementType elementType)
        {
            if (!_elementToBendingState.TryGetValue(elementType, out var bendingState)) return;
        
            ChangeState(bendingState);
            _spellsManager.OnSelectedElementChanged?.Invoke(elementType);
        }
    
        private void OnDestroy()
        {
            if (CurrentState != null)
            {
                CurrentState.EndState();
            }
        
            RemoveListeners();
        }
    }
}