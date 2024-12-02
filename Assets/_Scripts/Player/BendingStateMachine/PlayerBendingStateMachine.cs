using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBendingStateMachine : PlayerStateMachine
{
    [field: SerializeField] public List<SpellConfig> Spells { get; private set; }

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<ElementType> bendingStyles;
    
    private readonly Dictionary<ElementType, BendingState> _elementToBendingState = new();

    private Spell _spellToCast;
    private ObjectPoolingManager _objectPoolingManager;
    private SpellsManager _spellsManager;

    private const int INITIAL_BENDING_SLOT_NUMBER = 1;
    
    private BendingState CurrentBendingState => CurrentState as BendingState;
    
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
        _objectPoolingManager = ObjectPoolingManager.Instance;
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
        
        PlayerActions.CastSpell.started += UseSpell;
        PlayerActions.CastSpell.canceled += CancelSpell;
    }

    private void RemoveListeners()
    {
        PlayerActions.NumKeys.started -= OnNumKeyStarted;
        
        PlayerActions.CastSpell.started -= UseSpell; 
        PlayerActions.CastSpell.canceled -= CancelSpell;
    }

    
    private void FireSpell() // called through animation event
    {
        if (_spellToCast == null) return;
        if (!_spellToCast.CanBeLaunched) return;
        
        if (_spellToCast.SpellData.IsChildOfSpawnPoint)
        {
            _spellToCast.transform.parent = spawnPoint.transform;
        }
        
        _spellToCast.transform.position = spawnPoint.position;
        _spellToCast.transform.rotation = transform.rotation;
        
        _spellToCast.Launch();
    }

    private void UseSpell(InputAction.CallbackContext _)
    {
        _spellToCast = _objectPoolingManager.GetFromPool(CurrentBendingState.SelectedSpell);
        
        if (!_spellToCast.CanBeUsed) return;
            
        _spellToCast.Use(Animator);
    }

    private void CancelSpell(InputAction.CallbackContext _)
    {
        _spellToCast.Cancel();
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
    
    private void OnDestroy() => RemoveListeners();
}
