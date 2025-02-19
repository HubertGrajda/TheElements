using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/TriggerBehaviour/DealDamage", fileName = "New Deal Damage Trigger")]
    public class DealDamageTriggerBehaviour : TriggerBehaviour<IDamageable>
    {
        [SerializeField] private int damage;
        [SerializeField] private bool dealDamageInTime;
        [SerializeField] private ElementType elementType;
        
        protected override void OnTriggerableEnter(IDamageable damageable, BehaviourTrigger _)
        {
            DealDamage(damageable);
        }

        protected override void OnTriggerableStay(IDamageable damageable, BehaviourTrigger _)
        {
            if (!dealDamageInTime) return;
            
            DealDamage(damageable);
        }
        
        private void DealDamage(IDamageable damageable)
        {
            damageable.TakeDamage(damage, elementType);
        }
    }
}