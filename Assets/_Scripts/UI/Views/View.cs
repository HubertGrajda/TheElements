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

        private InputsManager _inputsManager;
        protected GameManager GameManager { get; private set; }
        protected UIManager UIManager { get; private set; }
    
        private bool IsShown { get; set; }
    
        protected virtual void Awake()
        {
            UIManager = UIManager.Instance;
            GameManager = GameManager.Instance;
            _inputsManager = InputsManager.Instance;
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
            _inputsManager.PlayerActions.Disable();
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
            _inputsManager.PlayerActions.Enable();
            GameManager.ResumeGame();
        
            containerUI.SetActive(false);
            IsShown = false;
            UIManager.OnViewClosed?.Invoke(this);
        }
    }
}