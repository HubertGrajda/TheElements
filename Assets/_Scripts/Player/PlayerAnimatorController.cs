using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        Managers.PlayerManager.death += OnDeath;
        Managers.PlayerManager.spellCastingStarted += OnSpellCastingStarted;
        Managers.PlayerManager.spellCastingCanceled += OnSpellCastingCanceled;
    }

    private void RemoveListeners()
    {
        Managers.PlayerManager.death -= OnDeath;
        Managers.PlayerManager.spellCastingStarted -= OnSpellCastingStarted;
        Managers.PlayerManager.spellCastingCanceled -= OnSpellCastingCanceled;
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
