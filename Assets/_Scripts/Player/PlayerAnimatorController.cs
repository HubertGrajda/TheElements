using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _anim;
    private PlayerManager _playerManager;
    private BaseHealthSystem _healthSystem;
    
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
        if (TryGetComponent(out _healthSystem))
        {
            _healthSystem.OnDeath += OnDeath;
        }
    }

    private void RemoveListeners()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDeath -= OnDeath;
        }
    }

    private void OnDeath()
    {
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        _anim.applyRootMotion = false;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
