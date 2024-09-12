using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _anim;
    private PlayerManager _playerManager;
    
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerManager = PlayerManager.Instance;
    }

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        _playerManager.death += OnDeath;
        _playerManager.spellCastingStarted += OnSpellCastingStarted;
        _playerManager.spellCastingCanceled += OnSpellCastingCanceled;
    }

    private void RemoveListeners()
    {
        _playerManager.death -= OnDeath;
        _playerManager.spellCastingStarted -= OnSpellCastingStarted;
        _playerManager.spellCastingCanceled -= OnSpellCastingCanceled;
    }

    private void OnDeath()
    {
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        _anim.applyRootMotion = false;
    }

    private void OnSpellCastingStarted(Spell spell)
    {
        spell.SpellData.CastingBehaviour.CasterStartCasting(_anim);
    }
    
    private void OnSpellCastingCanceled(Spell spell)
    {
        spell.SpellData.CastingBehaviour.CasterStopCasting(_anim);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
