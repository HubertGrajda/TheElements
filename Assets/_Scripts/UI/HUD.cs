using _Scripts.Managers;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class HUD : MonoBehaviour
    {
        private UIManager _uiManager;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            _uiManager = UIManager.Instance;
            
            AddListeners();
        }

        private void AddListeners()
        {
            _uiManager.OnViewClosed += OnViewClosed;
            _uiManager.OnViewOpened += OnViewOpened;
        }

        private void RemoveListeners()
        {
            if (_uiManager == null) return;
            
            _uiManager.OnViewClosed -= OnViewClosed;
            _uiManager.OnViewOpened -= OnViewOpened;
        }
        
        private void OnViewOpened(View _) => _canvas.enabled = false;
        private void OnViewClosed(View _) => _canvas.enabled = true;

        private void OnDestroy() => RemoveListeners();
    }
}