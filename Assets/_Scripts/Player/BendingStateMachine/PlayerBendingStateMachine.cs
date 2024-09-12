using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBendingStateMachine : PlayerStateMachine
{
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<WaterSpell> waterSpells;
    [SerializeField] private List<FireSpell> fireSpells;
    [SerializeField] private List<AirSpell> airSpells;
    [SerializeField] private List<EarthSpell> earthSpells;
    public List<WaterSpell> WaterSpells => waterSpells;
    public List<FireSpell> FireSpells => fireSpells;
    public List<AirSpell> AirSpells => airSpells;
    public List<EarthSpell> EarthSpells => earthSpells;

    private Spell _spellToCast;

    private WaterBendingState _waterBendingState;
    private FireBendingState _fireBendingState;
    private AirBendingState _airBendingState;
    private EarthBendingState _earthBendingState;

    private BendingState CurrentBendingState => CurrentState as BendingState;
    
    private ObjectPoolingManager _objectPoolingManager;
    
    private bool _onCooldown;
    
    protected override void InitStates(out State entryState)
    {
        _waterBendingState = new WaterBendingState(this);
        _fireBendingState = new FireBendingState(this);
        _airBendingState = new AirBendingState(this);
        _earthBendingState = new EarthBendingState(this);

        entryState = _waterBendingState;
    }

    protected override void Start()
    {
        base.Start();
        
        _objectPoolingManager = ObjectPoolingManager.Instance;
        
        AddListeners();
    }

    private void AddListeners()
    {
        PlayerActions.WaterBending.started += OnWaterBendingStarted;
        PlayerActions.EarthBending.started += OnEarthBendingStarted;
        PlayerActions.AirBending.started += OnAirBendingStarted;
        PlayerActions.FireBending.started += OnFireBendingStarted;
        
        PlayerActions.CastSpell.started += CastSpell;
        PlayerActions.CastSpell.canceled += CancelSpellCasting;
    }

    private void RemoveListeners()
    {
        PlayerActions.WaterBending.started -= OnWaterBendingStarted;
        PlayerActions.EarthBending.started -= OnEarthBendingStarted;
        PlayerActions.AirBending.started -= OnAirBendingStarted;
        PlayerActions.FireBending.started -= OnFireBendingStarted;
        
        PlayerActions.CastSpell.started -= CastSpell; 
        PlayerActions.CastSpell.canceled -= CancelSpellCasting;
    }

    
    private void FireSpell() // called through animation event
    {
        _spellToCast = _objectPoolingManager.SpawnFromPool(CurrentBendingState.SelectedSpell, spawnPoint.transform.position, transform.rotation);
        
        if (_spellToCast.SpellData.IsChildOfSpawnPoint)
        {
            _spellToCast.transform.parent = spawnPoint.transform;
        }
        
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
        if(_onCooldown) return;
        
        _spellToCast = _objectPoolingManager.GetFromPool(CurrentBendingState.SelectedSpell);
        StartCoroutine(Cooldown(_spellToCast.SpellData.Cooldown));
        
        PlayerManager.spellCastingStarted.Invoke(_spellToCast);
    }

    private void CancelSpellCasting(InputAction.CallbackContext _)
    {
        PlayerManager.spellCastingCanceled.Invoke(_spellToCast);
    }

    private void OnWaterBendingStarted(InputAction.CallbackContext _) => ChangeState(_waterBendingState);
    
    private void OnEarthBendingStarted(InputAction.CallbackContext _) => ChangeState(_earthBendingState);
    
    private void OnAirBendingStarted(InputAction.CallbackContext _) => ChangeState(_airBendingState);
    
    private void OnFireBendingStarted(InputAction.CallbackContext _) => ChangeState(_fireBendingState);

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
