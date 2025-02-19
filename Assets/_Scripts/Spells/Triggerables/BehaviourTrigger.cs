using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Collider))]
    public class BehaviourTrigger : MonoBehaviour
    {
        [SerializeField] protected List<TriggerBehaviourBase> triggerBehaviours;
        
        private readonly Dictionary<Collider, List<Triggerable>> _colliderToTriggerable = new();
        private readonly Dictionary<Triggerable, float> _triggerableToTimer = new();
        
        private class Triggerable
        {
            private readonly BehaviourTrigger _trigger;
            private readonly Component _component;
            public TriggerBehaviourBase Behaviour { get; }

            public Triggerable(Component component, TriggerBehaviourBase behaviour, BehaviourTrigger trigger)
            {
                Behaviour = behaviour;
                _component = component;
                _trigger = trigger;
            }

            public void OnTriggerableEnter() => Behaviour.OnTriggerableEnter(_component, _trigger);
            public void OnTriggerableExit() => Behaviour.OnTriggerableExit(_component, _trigger);
            public void OnTriggerableStay() => Behaviour.OnTriggerableStay(_component, _trigger);
        }
        
        protected void OnTriggerEnter(Collider other)
        {
            _colliderToTriggerable.TryAdd(other, new List<Triggerable>());
            
            foreach (var behaviour in triggerBehaviours)
            {
                var behaviourType = behaviour.GetType();
                
                if (behaviourType.BaseType?.IsGenericType != true) continue;
                
                var genericType = behaviourType.BaseType.GetGenericArguments()[0];
                    
                if (!other.TryGetComponent(genericType, out var triggerableComponent)) continue;

                var triggerable = new Triggerable(triggerableComponent, behaviour, this);
                triggerable.OnTriggerableEnter();
                
                _triggerableToTimer.Add(triggerable, triggerable.Behaviour.RetriggerTime);

                if (_colliderToTriggerable.TryGetValue(other, out var triggerables))
                {
                    triggerables.Add(triggerable);
                }
            }
        }
        
        protected void OnTriggerStay(Collider other)
        {
            if (!_colliderToTriggerable.TryGetValue(other, out var triggerables)) return;

            foreach (var triggerable in triggerables)
            {
                if (_triggerableToTimer.TryGetValue(triggerable, out var timer) && timer <= 0)
                {
                    _triggerableToTimer[triggerable] = triggerable.Behaviour.RetriggerTime;
                    triggerable.OnTriggerableStay();
                }
            
                _triggerableToTimer[triggerable] -= Time.deltaTime;
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (!_colliderToTriggerable.TryGetValue(other, out var triggerables)) return;

            foreach (var triggerable in triggerables)
            {
                triggerable.OnTriggerableExit();
                _triggerableToTimer.Remove(triggerable);
            }
            
            _colliderToTriggerable.Remove(other);
        }

        private void OnDisable() => ClearTriggerables();
        
        private void ClearTriggerables()
        {
            foreach (var triggerables in _colliderToTriggerable.Values)
            {
                foreach (var triggerable in triggerables)
                {
                    triggerable.OnTriggerableExit();
                }
            }
            
            _triggerableToTimer.Clear();
            _colliderToTriggerable.Clear();
        }
    }
}