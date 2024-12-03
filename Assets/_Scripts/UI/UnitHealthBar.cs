using UnityEngine;

namespace UI
{
    public class UnitHealthBar : HealthBar
    {
        [SerializeField] private BaseHealthSystem healthSystem;

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