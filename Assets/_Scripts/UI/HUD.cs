using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class HUD : MonoBehaviour
    {
        [field: SerializeField] public Image Crosshair { get; private set; }

        private UIManager _uiManager;
        private Canvas _canvas;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _uiManager = UIManager.Instance;
            _uiManager.AttachHUD(this);
        }

        private void Start()
        {
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