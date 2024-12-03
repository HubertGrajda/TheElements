namespace _Scripts.Managers
{
    public class InputsManager : Singleton<InputsManager>
    {
        private PlayerInputs _inputs;

        public PlayerInputs.UIActions UIActions => _inputs.UI;
        public PlayerInputs.PlayerActions PlayerActions => _inputs.Player;

        protected override void Awake()
        {
            base.Awake();

            _inputs = new PlayerInputs();
        }

        private void DisableInputs()
        {
            _inputs.Disable();
        }

        public void EnableInputs()
        {
            _inputs.Enable();
        }

        private void OnDestroy()
        {
            DisableInputs();
        }
    }
}