using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/TriggerBehaviour/DealDamage", fileName = "New Deal Damage Trigger")]
    public class DealDamageTriggerBehaviour : OnColliderTriggerBehaviour<IDamageable>
    {
        [SerializeField] private int damage;
        [SerializeField] private bool dealDamageInTime;
        [SerializeField] private ElementType elementType;
        
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
            damageable.TakeDamage(damage, elementType);
        }
    }
}