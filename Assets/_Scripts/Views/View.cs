using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class View : MonoBehaviour
    {
        [SerializeField] private GameObject containerUI;
    
        [SerializeField] private bool showCursor;
        [SerializeField] private bool pauseGame;
    
        public bool ShowCursor => showCursor;

        private InputManager _inputManager;
        protected GameManager GameManager { get; private set; }
        protected UIManager UIManager { get; private set; }
    
        private bool IsShown { get; set; }
    
        protected virtual void Start()
        {
            UIManager = UIManager.Instance;
            GameManager = GameManager.Instance;
            _inputManager = InputManager.Instance;
        }

        protected virtual void ToggleByInput(InputAction.CallbackContext context)
        {
            if (IsShown)
            {
                Hide();
                return;
            }
        
            Show();
        }
    
        protected virtual void Show()
        {
            _inputManager.PlayerActions.Disable();
            UIManager.CurrentView = this;
        
            if (pauseGame)
            {
                GameManager.PauseGame();
            }
        
            containerUI.SetActive(true);
            IsShown = true;
            UIManager.OnViewOpened?.Invoke(this);
        }

        public virtual void Hide()
        {
            UIManager.CurrentView = null;
            _inputManager.PlayerActions.Enable();
            GameManager.ResumeGame();
        
        
            containerUI.SetActive(false);
            IsShown = false;
            UIManager.OnViewClosed?.Invoke(this);
        }
    }
}