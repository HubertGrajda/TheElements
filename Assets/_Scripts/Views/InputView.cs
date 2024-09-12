using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public abstract class InputView : View
    {
        [SerializeField] private bool holdRequired;
        
        protected InputAction inputAction;
    
        protected override void Start()
        {
            base.Start();
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