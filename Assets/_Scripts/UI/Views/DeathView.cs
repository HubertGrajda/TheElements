using _Scripts.Managers;

namespace UI
{
    public class DeathView : View
    {
        private PlayerEvents _playerEvents;
        private PlayerManager _playerManager;
        private CameraManager _cameraManager;

        protected override void Awake()
        {
            base.Awake();
            
            _playerManager = PlayerManager.Instance;
            _cameraManager = CameraManager.Instance;
        }
        
        protected void Start() => AddListeners();

        private void AddListeners()
        {
            _playerManager.death += LaunchDeathView;
        }

        private void RemoveListeners()
        {
            if (_playerManager == null) return;
            
            _playerManager.death -= LaunchDeathView;
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