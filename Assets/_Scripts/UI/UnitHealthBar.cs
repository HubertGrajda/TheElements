using UnityEngine;

namespace _Scripts.UI
{
    public class UnitHealthBar : HealthBar
    {
        [SerializeField] private HealthSystem healthSystem;

        private void Start()
        {
            if (healthSystem == null) return;
            
            Init(healthSystem);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            gameObject.SetActive(false);
        }
    }
}