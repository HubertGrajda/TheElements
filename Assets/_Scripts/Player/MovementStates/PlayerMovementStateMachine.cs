using System;
using System.Collections;
using _Scripts.Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(GroundDetector))]
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class PlayerMovementStateMachine : PlayerStateMachine
    {
        public Action<JumpingState.JumpingSubState> OnJumpingSubStateChanged;
        public Action<bool> OnCrouchingStateChanged;
        
        [SerializeField] private float checkingForGroundDistance;
        [SerializeField] private LayerMask groundingLayers;
        [SerializeField] private PlayerMovementStatsConfig movementsStats;
    
        private Vector3 _playerVelocity;
        private float _turnSmoothVelocity;

        private float _targetAngle;
        private float _angle;

        private Vector2 _movementInput;
        
        private Transform _cameraTransform;
        public CharacterController CharacterController { get; private set; }
        public GroundDetector GroundDetector { get; private set; }
        
        public bool IsMovingInputActive => PlayerActions.Move.IsPressed();
        public bool IsCrouchingInputActive => PlayerActions.Crouch.IsPressed();
        public bool IsRunningInputActive => PlayerActions.Run.IsPressed();
        public bool IsJumpingInputActive => PlayerActions.Jump.IsPressed();
    
        public Vector3 PlayerVelocity => _playerVelocity;
        public float CurrentSpeed { get; private set; }

        public PlayerMovementStatsConfig MovementStats => movementsStats;

        private BaseHealthSystem _healthSystem;
        private Coroutine _movementSmoothingCoroutine;
        
        protected override void Awake()
        {
            base.Awake();
            
            CharacterController = GetComponent<CharacterController>();
            GroundDetector = GetComponent<GroundDetector>();
        
            AddListeners();
            PlayerActions.Enable();
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
        
        protected override void Start()
        {
            _cameraTransform = CameraManager.Instance.CameraMain.transform;
            
            base.Start();
            SetGravity();
        }

        protected override void InitStates(out State entryState)
        {
            States.Add(new IdleState(this));
            States.Add(new JumpingState(this));
            States.Add(new WalkingState(this));
            States.Add(new RunningState(this));
            States.Add(new CrouchingState(this));
            
            entryState = States[0];
        }
    
        protected override void Update()
        {
            base.Update();
            ApplyGravity();
        }
    
        private void ApplyGravity()
        {
            CharacterController.Move(_playerVelocity * Time.deltaTime);
        }

        public void SetCurrentSpeed(float speed)
        {
            if (Mathf.Approximately(CurrentSpeed, speed)) return;
            
            if (_movementSmoothingCoroutine != null)
            {
                StopCoroutine(_movementSmoothingCoroutine);
            }

            _movementSmoothingCoroutine = StartCoroutine(MovementSmoothing(speed));
        }
        
        private IEnumerator MovementSmoothing(float speed)
        {
            while (!Mathf.Approximately(CurrentSpeed, speed))
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, speed, movementsStats.AccelerationSpeed * Time.deltaTime);
                PlayerAnimatorController.OnMovementSpeedChanged(CurrentSpeed);
                yield return null;
            }
            
            CurrentSpeed = speed;
            PlayerAnimatorController.OnMovementSpeedChanged(CurrentSpeed);
            _movementSmoothingCoroutine = null;
        }

        public void AddVelocityY(float velocity) => _playerVelocity.y += velocity;
        
        public void SetVelocity(Vector3 velocity) => _playerVelocity = velocity;

        public Vector3 SetDirection()
        {
            var movementDirection = PlayerActions.Move.ReadValue<Vector2>();

            _targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, movementsStats.TurnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, _angle, 0f);
            
            return Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;;
        }

        private void SetGravity()
        {
            if (GroundDetector.IsGrounded)
            {
                _playerVelocity.y = movementsStats.GroundedGravity;
            }
        }

        private void PlayFootstepSound(string soundName)
        {
            AudioManager.Instance.PlaySound(soundName);
        }

        private void OnDeath()
        {
            CharacterController.enabled = false;
        }

        private void OnDestroy() => RemoveListeners();
    }
}