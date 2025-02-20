using UnityEngine;

namespace _Scripts.UI
{
    public abstract class Menu : MonoBehaviour
    {
        private bool _initialized;
        protected UIManager UIManager { get; private set; }
    
        protected virtual void Start()
        {
            UIManager = UIManager.Instance;
        
            InitUIElements();
            _initialized = true;
        }

        public virtual void Open()
        {
            if (!_initialized)
            {
                Start();
            }

            if (UIManager.CurrentMenu != null)
            {
                UIManager.CurrentMenu.Close();
            }
        
            gameObject.SetActive(true);
            UIManager.SetCurrentMenu(this);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            UIManager.SetCurrentMenu(null);
        }
        
        protected abstract void InitUIElements();

        protected void OnBackButtonClicked()
        {
            UIManager.OpenPreviousMenu();
        }
    }
}