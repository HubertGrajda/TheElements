using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.UI
{
    public abstract class InputView : View
    {
        [SerializeField] private bool holdRequired;
        
        protected InputAction inputAction;
    
        protected void Start()
        {
            AssignInputAction();
            AddListeners();
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
        
        protected virtual void AddListeners()
        {
            if (inputAction == null) return;
            
            inputAction.started += ToggleByInput;
        
            if (holdRequired)
            {
                inputAction.canceled += ToggleByInput;
            }
        }

        protected virtual void RemoveListeners()
        {
            if (inputAction == null) return;
            
            inputAction.started -= ToggleByInput;
        
            if (holdRequired)
            {
                inputAction.canceled -= ToggleByInput;
            }
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        protected abstract void AssignInputAction();
    }
}