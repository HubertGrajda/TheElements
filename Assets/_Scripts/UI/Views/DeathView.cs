using _Scripts.Cameras;
using _Scripts.Player;

namespace _Scripts.UI
{
    public class DeathView : View
    {
        private PlayerEvents _playerEvents;
        private PlayerManager _playerManager;
        private CameraManager _cameraManager;
        private HealthSystem _playerHealthSystem;
        
        protected override void Awake()
        {
            base.Awake();
            
            _playerManager = PlayerManager.Instance;
            _cameraManager = CameraManager.Instance;
        }
        
        protected void Start() => AddListeners();

        private void AddListeners()
        {
            if (_playerManager.TryGetPlayerComponent(out _playerHealthSystem))
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
            UIManager.HideCurrentView();
            Show();
        }

        public override void Hide()
        {
            if (!IsShown) return;
            
            base.Hide();
        
            _cameraManager.ToggleMainCameraMovement(true);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}