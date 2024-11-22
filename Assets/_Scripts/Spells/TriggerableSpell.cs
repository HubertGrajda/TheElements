using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Spells
{
    public class TriggerableSpell : MonoBehaviour // TODO: Overkill - solve OnTriggerStay GetComponent problem
    {
        [SerializeField] protected List<OnColliderTriggerBehaviourBase> triggerBehaviours;
        
        private readonly Dictionary<Collider, Triggerable> _colliderToTriggerable = new();
        private float _timer;
        
        private class Triggerable
        {
            private readonly Component _component;
            public OnColliderTriggerBehaviourBase Behaviour { get; }

            public Triggerable(Component component, OnColliderTriggerBehaviourBase behaviour)
            {
                Behaviour = behaviour;
                _component = component;
            }

            public void OnTriggerableEnter() => Behaviour.OnTriggerableEnter(_component);
            public void OnTriggerableExit() => Behaviour.OnTriggerableExit(_component);
            public void OnTriggerableStay() => Behaviour.OnTriggerableStay(_component);
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            foreach (var behaviour in triggerBehaviours)
            {
                var behaviourType = behaviour.GetType();
                
                if (behaviourType.BaseType?.IsGenericType != true) continue;
                
                var genericType = behaviourType.BaseType.GetGenericArguments()[0];
                    
                if (!other.TryGetComponent(genericType, out var triggerableComponent)) continue;

                var triggerable = new Triggerable(triggerableComponent, behaviour);
                triggerable.OnTriggerableEnter();
                
                _colliderToTriggerable.Add(other, triggerable);
            }
        }
        
        protected virtual void OnTriggerStay(Collider other)
        {
            if (!_colliderToTriggerable.TryGetValue(other, out var triggerable)) return;
            
            if (_timer <= 0)
            {
                _timer = triggerable.Behaviour.RetriggerTime;
                triggerable.OnTriggerableStay();
            }
            
            _timer -= Time.deltaTime;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!_colliderToTriggerable.TryGetValue(other, out var triggerable)) return;
            
            triggerable.OnTriggerableExit();
            _colliderToTriggerable.Remove(other);
        }

        private void OnDisable() => ClearTriggerables();
        
        public void ClearTriggerables()
        {
            foreach (var triggerable in _colliderToTriggerable.Values)
            {
                triggerable.OnTriggerableExit();
            }
            
            _colliderToTriggerable.Clear();
        }
    }
}