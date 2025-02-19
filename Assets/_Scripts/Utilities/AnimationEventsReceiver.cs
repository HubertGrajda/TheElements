using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class AnimationEventsReceiver : MonoBehaviour
    {
        [SerializeField] private List<AnimationEvent> animationEvents;
    
        public void Raise(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
        
            var eventToRaise = animationEvents.FirstOrDefault(x => x.EventName == eventName);

            eventToRaise?.Invoke();
        }
    
        [Serializable]
        private class AnimationEvent
        {
            [field: SerializeField] public string EventName { get; private set; }
        
            [SerializeField] private UnityEvent unityEvent;

            public void Invoke() => unityEvent?.Invoke();
        }
    }
}