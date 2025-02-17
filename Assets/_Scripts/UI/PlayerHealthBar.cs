using _Scripts.Player;
using UnityEngine;

namespace _Scripts.UI
{
    public class PlayerHealthBar : HealthBar
    {
        [SerializeField] private QuickTextVisualizer damageTextVisualizer;
        
        private HealthSystem healthSystem;

        private void Start()
        {
            if (PlayerManager.Instance.TryGetPlayerComponent(out healthSystem))
            {
                Init(healthSystem);
                healthSystem.OnDamaged += OnDamaged;
            }
        }

        private void OnDestroy()
        {
            if (healthSystem == null) return;
            
            healthSystem.OnDamaged -= OnDamaged;
        }
        
        private void OnDamaged(int damage)
        {
            if (damageTextVisualizer != null)
            {
                damageTextVisualizer.Show($"-{damage}");
            }
        }
    }
}