using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBendingStateMachine : PlayerStateMachine
{
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<SpellConfig> spells;
    [SerializeField] private List<ElementType> bendingStyles;
    
    public List<SpellConfig> Spells => spells;
    private BendingState CurrentBendingState => CurrentState as BendingState;

    private Spell _spellToCast;
    private ObjectPoolingManager _objectPoolingManager;
    private bool _onCooldown;
    private readonly Dictionary<ElementType, BendingState> _elementToBendingState = new();
    
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
        base.Start();
        
        _objectPoolingManager = ObjectPoolingManager.Instance;
        
        AddListeners();
    }

    private void AddListeners()
    {
        PlayerActions.NumKeys.started += OnNumKeyStarted;
        
        PlayerActions.CastSpell.started += CastSpell;
        PlayerActions.CastSpell.canceled += CancelSpellCasting;
    }

    private void RemoveListeners()
    {
        PlayerActions.NumKeys.started -= OnNumKeyStarted;
        
        PlayerActions.CastSpell.started -= CastSpell; 
        PlayerActions.CastSpell.canceled -= CancelSpellCasting;
    }

    
    private void FireSpell() // called through animation event
    {
        _spellToCast = _objectPoolingManager.GetFromPool(CurrentBendingState.SelectedSpell);

        if (!_spellToCast.CanBeCasted) return;
        
        if (_spellToCast.SpellData.IsChildOfSpawnPoint)
        {
            _spellToCast.transform.parent = spawnPoint.transform;
        }
        
        _spellToCast.transform.position = spawnPoint.position;
        _spellToCast.transform.rotation = transform.rotation;
        
        _spellToCast.CastSpell();
    }

    private IEnumerator Cooldown(float time)
    {
        _onCooldown = true;
        
        var timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        _onCooldown = false;
    }
    
    private void CastSpell(InputAction.CallbackContext _)
    {
        if (_onCooldown) return;
        
        _spellToCast = _objectPoolingManager.GetFromPool(CurrentBendingState.SelectedSpell);
        StartCoroutine(Cooldown(_spellToCast.SpellData.Cooldown));
        
        PlayerManager.spellCastingStarted.Invoke(_spellToCast);
    }

    private void CancelSpellCasting(InputAction.CallbackContext _)
    {
        PlayerManager.spellCastingCanceled.Invoke(_spellToCast);
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
        if (!_elementToBendingState.TryGetValue(bendingStyles[slotIndex], out var bendingState)) return;
        
        ChangeState(bendingState);
    }
    
    private void OnDestroy()
    {
        RemoveListeners();
    }
}
