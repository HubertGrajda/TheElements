using _Scripts.Spells;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private PlayerController _playerRef;

        public PlayerController PlayerRef
        {
            get
            {
                if (_playerRef == null)
                {
                    var playerFound = FindAnyObjectByType<PlayerController>();
                    SetUpPlayerRef(playerFound);
                }

                return _playerRef;
            }
        }

        public UnityAction death;
        public UnityAction<Spell> spellCastingStarted;
        public UnityAction<Spell> spellCastingCanceled;


        private PlayerExperienceSystem _experienceSystem;
        public PlayerExperienceSystem ExperienceSystem => _experienceSystem;

        public Transform PlayerTransform => PlayerRef.transform;

        public void SetUpPlayerRef(PlayerController player)
        {
            _playerRef = player;
            _playerRef.TryGetComponent(out _experienceSystem);
        }
    }
}