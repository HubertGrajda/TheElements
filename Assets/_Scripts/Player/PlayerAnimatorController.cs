using _Scripts.Managers;
using Player;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(BaseHealthSystem))]
[RequireComponent(typeof(PlayerMovementStateMachine))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _anim;
    private PlayerManager _playerManager;
    private BaseHealthSystem _healthSystem;
    private GroundDetector _groundDetector;
    private PlayerMovementStateMachine _playerMovementStateMachine;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _groundDetector = GetComponent<GroundDetector>();
        _healthSystem = GetComponent<BaseHealthSystem>();
        _playerMovementStateMachine = GetComponent<PlayerMovementStateMachine>();
    }

    private void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        _playerMovementStateMachine.OnCrouchingStateChanged += OnCrouchingChanged;
        _playerMovementStateMachine.OnJumpingSubStateChanged += OnJumpingSubStateChanged;
        _groundDetector.OnGroundedChanged += OnGroundedChanged;
        _healthSystem.OnDeath += OnDeath;
    }

    private void RemoveListeners()
    {
        _playerMovementStateMachine.OnJumpingSubStateChanged -= OnJumpingSubStateChanged;
        _groundDetector.OnGroundedChanged -= OnGroundedChanged;
        _healthSystem.OnDeath -= OnDeath;
    }
    
    
#region Movement

    private void OnGroundedChanged(bool isGrounded)
    {
        _anim.SetBool(Constants.AnimationNames.GROUNDED, isGrounded);
    }
    
    private void OnJumpingSubStateChanged(JumpingState.JumpingSubState state)
    {
        _anim.SetInteger(Constants.AnimationNames.JUMP, (int)state);
    }

    private void OnCrouchingChanged(bool isCrouching)
    {
        _anim.SetBool(Constants.AnimationNames.CROUCH, isCrouching);
    }
    
    public void OnMovementSpeedChanged(float speed)
    {
        _anim.SetFloat(Constants.AnimationNames.MOVEMENT_SPEED, speed);
    }

#endregion
    
    private void OnDeath()
    {
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        _anim.applyRootMotion = false;
    }

    private void OnDestroy() => RemoveListeners();
}
