
namespace _Scripts.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private PlayerController _playerRef;

        private PlayerExperienceSystem _experienceSystem;
        public PlayerExperienceSystem ExperienceSystem => _experienceSystem;

        public void SetUpPlayerRef(PlayerController player)
        {
            _playerRef = player;
            _playerRef.TryGetComponent(out _experienceSystem);
        }

        public bool TryGetPlayerController(out PlayerController playerController)
        {
            playerController = default;
            
            if (_playerRef != null)
            {
                playerController = _playerRef;
                return true;
            }
            
            var playerFound = FindAnyObjectByType<PlayerController>();

            if (playerFound == null) return false;
                
            SetUpPlayerRef(playerFound);

            return _playerRef != true;
        }
    }
}