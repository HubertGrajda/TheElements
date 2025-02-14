using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerBendingStateMachine : PlayerStateMachine
    {
        [SerializeField] private Transform spawnPoint;
    
        private readonly Dictionary<ElementType, BendingState> _elementToBendingState = new();

        private SpellsManager _spellsManager;

        private const int INITIAL_BENDING_SLOT_NUMBER = 1;
    
        public BendingState CurrentBendingState => CurrentState as BendingState;
    
        protected override void InitStates(out State entryState)
        {
            entryState = default;
            
            foreach (var elementType in _spellsManager.BendingStyles)
            {
                _elementToBendingState.Add(elementType, new BendingState(this, elementType));
            }
            
            if (_elementToBendingState.Count == 0) return;
            
            entryState = _elementToBendingState.First().Value;
        }

        protected override void Awake()
        {
            base.Awake();
            _spellsManager = SpellsManager.Instance;
        }

        protected override void Start()
        {
            base.Start();

            OnBendingSlotStarted(INITIAL_BENDING_SLOT_NUMBER);
            AddListeners();
        }

        private void AddListeners()
        {
            PlayerActions.NumKeys.started += OnNumKeyStarted;
            _spellsManager.OnBendingStyleUnlocked += OnBendingStyleUnlocked;
        }

        private void RemoveListeners()
        {
            PlayerActions.NumKeys.started -= OnNumKeyStarted;
            _spellsManager.OnBendingStyleUnlocked -= OnBendingStyleUnlocked;
        }
    
        private void OnBendingStyleUnlocked(ElementType elementType)
        {
            var bendingState = new BendingState(this, elementType);
            _elementToBendingState.Add(elementType, bendingState);
            ChangeState(bendingState);
        }
        
        private void OnNumKeyStarted(InputAction.CallbackContext context)
        {
            var value = (int)context.ReadValue<float>();
        
            OnBendingSlotStarted(value);
        }
    
        private void OnBendingSlotStarted(int slotNumber)
        {
            var activeElementType = _spellsManager.BendingStyles.FirstOrDefault(x => x.Index == slotNumber);
            
            if (activeElementType == null) return;

            ChangeState(activeElementType);
        }

        public void ChangeState(ElementType elementType)
        {
            if (!_elementToBendingState.TryGetValue(elementType, out var bendingState)) return;
        
            ChangeState(bendingState);
            _spellsManager.OnActiveElementChanged?.Invoke(elementType);
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