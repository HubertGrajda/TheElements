

public class InputManager : Singleton<InputManager>
{
    private PlayerInputs _inputs;
    public PlayerInputs Inputs => _inputs;

    public PlayerInputs.UIActionsActions UIActions => _inputs.UIActions;
    public PlayerInputs.PlayerActionsActions PlayerActions => _inputs.PlayerActions;
    
    protected override void Awake()
    {
        base.Awake();

        _inputs = new PlayerInputs();
    }

    public void DisableInputs()
    {
        _inputs.Disable();
    }

    public void EnableInputs()
    {
        _inputs.Enable();
    }
}
