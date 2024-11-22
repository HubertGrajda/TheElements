using UnityEngine.InputSystem;

public interface IInputInteractable
{
    public void InteractionBehaviour(InputAction.CallbackContext context);
}

public interface IAirInteractable
{
    public void OnInteractionStart();
    public void OnInteractionEnd();
}

public interface ICollectable
{
    public void Collect();
}
