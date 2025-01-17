using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.UI
{
    public class MiniMap : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private Transform _followTarget;
        private Transform _rotationTarget;
        
        private void Start()
        {
            _playerManager = PlayerManager.Instance;
            _playerManager.TryGetPlayerComponent(out _followTarget);
            _rotationTarget = CameraManager.Instance.CameraMain.transform;
        }

        private void LateUpdate()
        {
            if (_followTarget == null || _rotationTarget == null) return;

            var position = _followTarget.position;

            transform.position = new Vector3(position.x, transform.position.y, position.z);
            transform.rotation = Quaternion.Euler(90f, _rotationTarget.eulerAngles.y, 0f);
        }
    }
}