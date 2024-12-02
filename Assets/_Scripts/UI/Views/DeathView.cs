using _Scripts.Managers;

namespace UI
{
    public class DeathView : View
    {
        private PlayerEvents _playerEvents;
        private PlayerManager _playerManager;
        private CameraManager _cameraManager;
        private BaseHealthSystem _playerHealthSystem;
        
        protected override void Awake()
        {
            base.Awake();
            
            _playerManager = PlayerManager.Instance;
            _cameraManager = CameraManager.Instance;
        }
        
        protected void Start() => AddListeners();

        private void AddListeners()
        {
            if (_playerManager.TryGetPlayerController(out var playerController) &&
                playerController.TryGetComponent(out _playerHealthSystem))
            {
                _playerHealthSystem.OnDeath += LaunchDeathView;
            }
        }

        private void RemoveListeners()
        {
            if (_playerHealthSystem == null) return;
            
            _playerHealthSystem.OnDeath -= LaunchDeathView;
        }
    
        private void LaunchDeathView()
        {
            _cameraManager.ToggleMainCameraMovement(false);
            Show();
        }

        public override void Hide()
        {
            base.Hide();
        
            _cameraManager.ToggleMainCameraMovement(true);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}