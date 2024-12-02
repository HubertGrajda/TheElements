using _Scripts.Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementStateMachine : PlayerStateMachine
    {
        [SerializeField] private float checkingForGroundDistance;
        [SerializeField] private LayerMask groundingLayers;
        [SerializeField] private PlayerMovementStatsConfig movementsStats;
    
        private Vector3 _playerVelocity;
        private float _turnSmoothVelocity;

        private float _targetAngle;
        private float _angle;

        private Vector2 _movementInput;
        
        private Transform _cameraTransform;
        private CharacterController _characterController;
        
        public IdleState IdleState { get; private set; }
        public JumpingState JumpingState { get; private set; }
        public WalkingState WalkingState { get; private set; }
        public RunningState RunningState { get; private set; }
        public CrouchingState CrouchingState { get; private set; }

        public bool IsMovingInputActive => PlayerActions.Move.IsPressed();
        public bool IsCrouchingInputActive => PlayerActions.Crouch.IsPressed();
        public bool IsRunningInputActive => PlayerActions.Run.IsPressed();
        public bool IsJumpingInputActive => PlayerActions.Jump.IsPressed();
    
        public Vector3 PlayerVelocity => _playerVelocity;
        public bool IsGrounded => IsPlayerGrounded();
        public float CurrentSpeed { get; private set; }

        public PlayerMovementStatsConfig MovementStats => movementsStats;
        
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
            PlayerManager.death += OnDeath;
        }

        private void RemoveListeners()
        {
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

            Animator.SetBool(Constants.AnimationNames.GROUNDED, IsGrounded);
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

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}