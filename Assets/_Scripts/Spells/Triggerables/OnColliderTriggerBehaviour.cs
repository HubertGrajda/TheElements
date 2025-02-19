using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class TriggerBehaviour<TComponent> : TriggerBehaviourBase
    {
        public sealed override void OnTriggerableEnter(Component other, BehaviourTrigger trigger)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableEnter(component, trigger);
        }

        public sealed override void OnTriggerableExit(Component other, BehaviourTrigger trigger)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableExit(component, trigger);
        }

        public sealed override void OnTriggerableStay(Component other, BehaviourTrigger trigger)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableStay(component, trigger);
        }

        protected virtual void OnTriggerableEnter(TComponent other, BehaviourTrigger trigger)
        {
        }

        protected virtual void OnTriggerableExit(TComponent other, BehaviourTrigger trigger)
        {
        }

        protected virtual void OnTriggerableStay(TComponent other, BehaviourTrigger trigger)
        {
        }
    }

    public abstract class TriggerBehaviourBase : ScriptableObject
    {
        [field: SerializeField] public float RetriggerTime { get; private set; }
        
        public abstract void OnTriggerableEnter(Component other, BehaviourTrigger trigger);
        public abstract void OnTriggerableExit(Component other, BehaviourTrigger trigger);
        public abstract void OnTriggerableStay(Component other, BehaviourTrigger trigger);
    }
}