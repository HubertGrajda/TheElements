using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/Deal Damage Trigger", fileName = "New Deal Damage Trigger")]
    public class DealDamageTriggerBehaviour : OnColliderTriggerBehaviour<IDamageable>
    {
        [SerializeField] private int damage;
        [SerializeField] private bool dealDamageInTime;
        
        protected override void OnTriggerableEnter(IDamageable other)
        {
            DealDamage(other);
        }

        protected override void OnTriggerableExit(IDamageable other)
        {
        }

        protected override void OnTriggerableStay(IDamageable other)
        {
            if (!dealDamageInTime) return;
            
            DealDamage(other);
        }
        
        private void DealDamage(IDamageable damageable)
        {
            damageable.TakeDamage(damage);
        }
    }
}