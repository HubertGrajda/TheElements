using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class OnColliderTriggerBehaviour<TComponent> : OnColliderTriggerBehaviourBase
    {
        public override void OnTriggerableEnter(Component other)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableEnter(component);
        }

        public override void OnTriggerableExit(Component other)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableExit(component);
        }

        public override void OnTriggerableStay(Component other)
        {
            if (other is not TComponent component) return;
            
            OnTriggerableStay(component);
        }
        
        protected abstract void OnTriggerableEnter(TComponent other);
        protected abstract void OnTriggerableExit(TComponent other);
        protected abstract void OnTriggerableStay(TComponent other);
        
    }

    public abstract class OnColliderTriggerBehaviourBase : ScriptableObject
    {
        [field: SerializeField] public float RetriggerTime { get; private set; }
        
        public abstract void OnTriggerableEnter(Component other);
        public abstract void OnTriggerableExit(Component other);
        public abstract void OnTriggerableStay(Component other);
    }
}