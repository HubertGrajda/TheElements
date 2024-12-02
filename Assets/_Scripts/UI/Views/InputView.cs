using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
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

        private void AddListeners()
        {
            inputAction.started += ToggleByInput;
        
            if (holdRequired)
            {
                inputAction.canceled += ToggleByInput;
            }
        }

        private void RemoveListeners()
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