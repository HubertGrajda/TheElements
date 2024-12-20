using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerManager _playerManager;
    
        private void Awake()
        {
            _playerManager = PlayerManager.Instance;
            _playerManager.SetUpPlayerComponent(this);
        }

        private void OnDestroy()
        {
            _playerManager.Clear();
        }
    }
}