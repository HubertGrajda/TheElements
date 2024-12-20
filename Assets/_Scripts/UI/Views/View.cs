using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.UI
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
    
        protected bool IsShown { get; private set; }
        protected bool CanBeShown => UIManager.CurrentView == null;
        
        protected virtual void Awake()
        {
            UIManager = UIManager.Instance;
            GameManager = GameManager.Instance;
            _inputsManager = InputsManager.Instance;
        }
    
        protected virtual void Show()
        {
            if (!CanBeShown) return;
            
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
            if (!IsShown) return;
            
            UIManager.CurrentView = null;
            _inputsManager.PlayerActions.Enable();

            if (pauseGame)
            {
                GameManager.ResumeGame();
            }

            containerUI.SetActive(false);
            IsShown = false;
            UIManager.OnViewClosed?.Invoke(this);
        }
    }
}