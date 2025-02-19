using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/TriggerBehaviour/AddForce", fileName = "New Add Force Trigger")]
    public class AddForceTriggerBehaviour : TriggerBehaviour<IKnockbackable>
    {
        [SerializeField] private float strength;
        [SerializeField] private float duration;
        
        protected override void OnTriggerableEnter(IKnockbackable knockbackable, BehaviourTrigger trigger)
        {
            ApplyKnockback(knockbackable, trigger.transform.forward);
        }

        private void ApplyKnockback(IKnockbackable knockbackable, Vector3 direction)
        {
            knockbackable.ApplyKnockback(direction * strength, duration);
        }
    }
}

public interface IKnockbackable
{
    void ApplyKnockback(Vector3 force, float duration);
}