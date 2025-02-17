using UnityEngine.InputSystem;

namespace _Scripts
{
    public interface IInputInteractable
    {
        public void InteractionBehaviour(InputAction.CallbackContext context);
    }
}
