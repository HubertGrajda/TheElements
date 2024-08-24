using _Scripts.Spells;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private GameObject _playerRef;
        public GameObject PlayerRef => _playerRef != null 
            ? _playerRef 
            : _playerRef = FindAnyObjectByType<PlayerStateMachine>().gameObject;

        public UnityAction death;
        public UnityAction<Spell> spellCastingStarted;
        public UnityAction<Spell> spellCastingCanceled;
        
        public void SetUpPlayerRef(GameObject player)
        {
            _playerRef = player;
        }
    }
}