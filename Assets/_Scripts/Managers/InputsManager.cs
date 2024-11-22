
namespace _Scripts.Managers
{
    public class InputsManager : Singleton<InputsManager>
    {
        public PlayerInputs Inputs { get; private set; }

        public PlayerInputs.UIActions UIActions => Inputs.UI;
        public PlayerInputs.PlayerActions PlayerActions => Inputs.Player;

        protected override void Awake()
        {
            base.Awake();

            Inputs = new PlayerInputs();
        }

        private void DisableInputs()
        {
            Inputs.Disable();
        }

        public void EnableInputs()
        {
            Inputs.Enable();
        }

        private void OnDestroy()
        {
            DisableInputs();
        }
    }
}