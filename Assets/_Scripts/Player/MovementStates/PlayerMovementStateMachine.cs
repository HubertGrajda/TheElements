using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementStateMachine : PlayerStateMachine
    {
        [SerializeField] private float checkingForGroundDistance;
        [SerializeField] private LayerMask groundingLayers;
        [SerializeField] private PlayerMovementStats_SO movementsStats;
    
        private Vector3 _playerVelocity;
        private float _turnSmoothVelocity;

        private float _targetAngle;
        private float _angle;

        public IdleState IdleState { get; private set; }
        public JumpingState JumpingState { get; private set; }
        public WalkingState WalkingState { get; private set; }
        public RunningState RunningState { get; private set; }
        public CrouchingState CrouchingState { get; private set; }

        ////// Inputs ///////
        private Vector2 _movementInput;

        public bool IsCrouchingInputActive { get; private set; }
        public bool IsMovingInputActive { get; private set; }
        public bool IsRunningInputActive { get; private set; }
        public bool IsJumpingInputActive { get; private set; }

        private Transform _cameraTransform;
        private CharacterController _characterController;
    
        public Vector3 PlayerVelocity => _playerVelocity;
        public bool IsGrounded => IsPlayerGrounded();
        public float CurrentSpeed { get; private set; }

        public PlayerMovementStats_SO MovementStats => movementsStats;
        
        protected override void Awake()
        {
            base.Awake();
        
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
        
            AddListeners();
            PlayerActions.Enable();
        }

        private void AddListeners()
        {
            PlayerActions.Jump.started += OnJumpInput;
            PlayerActions.Jump.canceled += OnJumpInput;
        
            PlayerActions.Run.started += OnRunInput;
            PlayerActions.Run.canceled += OnRunInput;
        
            PlayerActions.Crouch.started += OnCrouchInput;
            PlayerActions.Crouch.canceled += OnCrouchInput;
        
            PlayerActions.Move.performed += OnMoveInput;
            PlayerActions.Move.started += OnMoveInput;
            PlayerActions.Move.canceled += OnMoveInput;

            PlayerManager.death += OnDeath;
        }

        private void RemoveListeners()
        {
            PlayerActions.Jump.started -= OnJumpInput;
            PlayerActions.Jump.canceled -= OnJumpInput;
        
            PlayerActions.Run.started -= OnRunInput;
            PlayerActions.Run.canceled -= OnRunInput;
        
            PlayerActions.Crouch.started -= OnCrouchInput;
            PlayerActions.Crouch.canceled -= OnCrouchInput;
        
            PlayerActions.Move.performed -= OnMoveInput;
            PlayerActions.Move.started -= OnMoveInput;
            PlayerActions.Move.canceled -= OnMoveInput;
        
            PlayerManager.death -= OnDeath;
        }
        protected override void Start()
        {
            base.Start();
        
            SetGravity();
        }

        protected override void InitStates(out State entryState)
        {
            IdleState = new IdleState(this);
            JumpingState = new JumpingState(this);
            WalkingState = new WalkingState(this);
            RunningState = new RunningState(this);
            CrouchingState = new CrouchingState(this);

            entryState = IdleState;
        }
    
        protected override void Update()
        {
            base.Update();
            Move();
        }
    
        private void Move()
        {
            _characterController.Move(SetDirection() * (CurrentSpeed * Time.deltaTime));
            _characterController.Move(_playerVelocity * Time.deltaTime);

            Anim.SetBool(Constants.AnimationNames.GROUNDED, IsGrounded);
        }

        public void SetCurrentSpeed(float speed)
        {
            CurrentSpeed = speed;
        }

        public void AddVelocityY(float velocity)
        {
            _playerVelocity.y += velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _playerVelocity = velocity;
        }

        private Vector3 SetDirection()
        {
            var dir = PlayerActions.Move.ReadValue<Vector2>();

            _targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, movementsStats.turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, _angle, 0f);
            return Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;;
        }

        private void SetGravity()
        {
            if (CurrentState != JumpingState)
            {
                _playerVelocity.y = movementsStats.groundedGravity;
            }
        }

        private bool IsPlayerGrounded()
        {
            var capsuleCenter = transform.position + Vector3.up;
        
            return Physics.CapsuleCast(capsuleCenter, capsuleCenter,
                _characterController.radius, Vector3.down, checkingForGroundDistance, groundingLayers);
        }

        private void PlayFootstepSound(string soundName) //function call in animation event
        {
            AudioManager.Instance.PlaySound(soundName);
        }

        private void OnDeath()
        {
            _characterController.enabled = false;
        }
    
        private void OnJumpInput(InputAction.CallbackContext context)
        {
            IsJumpingInputActive = context.ReadValueAsButton();
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
            IsMovingInputActive = _movementInput.x != 0f || _movementInput.y != 0f;
        }

        private void OnCrouchInput(InputAction.CallbackContext context)
        {
            IsCrouchingInputActive = context.ReadValueAsButton();
        }
    
        private void OnRunInput(InputAction.CallbackContext context)
        {
            IsRunningInputActive = context.ReadValueAsButton();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}