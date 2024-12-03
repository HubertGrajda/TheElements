using _Scripts.Managers;

namespace UI
{
    public class PlayerHealthBar : HealthBar
    {
        private BaseHealthSystem healthSystem;

        private void Start()
        {
            if (PlayerManager.Instance.TryGetPlayerComponent(out healthSystem))
            {
                Init(healthSystem);
            }
        }
    }
}