using _Scripts.Managers;

namespace _Scripts.UI
{
    public class PlayerHealthBar : HealthBar
    {
        private HealthSystem healthSystem;

        private void Start()
        {
            if (PlayerManager.Instance.TryGetPlayerComponent(out healthSystem))
            {
                Init(healthSystem);
            }
        }
    }
}