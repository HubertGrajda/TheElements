using _Scripts.Managers;

namespace UI
{
    public class DeathView : View
    {
        private PlayerEvents _playerEvents;
        private PlayerManager _playerManager;
        private CameraManager _cameraManager;
    
        protected override void Start()
        {
            base.Start();
        
            _playerManager = PlayerManager.Instance;
            _cameraManager = CameraManager.Instance;
        
            AddListeners();
        }

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