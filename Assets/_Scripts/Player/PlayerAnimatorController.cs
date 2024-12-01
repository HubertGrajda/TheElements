using _Scripts.Managers;
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
    }

    private void RemoveListeners()
    {
        _playerManager.death -= OnDeath;
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
