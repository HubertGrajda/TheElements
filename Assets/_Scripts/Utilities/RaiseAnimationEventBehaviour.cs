using UnityEngine;

namespace _Scripts
{
    public class RaiseAnimationEventBehaviour : StateMachineBehaviour
    {
        [Tooltip("0 - casted on start, 1 - casted on exit")]
        [SerializeField, Range(0f,1f)] private float castTime;
        [SerializeField] private string eventName;
 
        private bool _raised;
        private AnimationEventsReceiver _eventsReceiver;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out _eventsReceiver)) return;
    
            if (castTime <= 0f)
            {
                RaiseEvent();
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_eventsReceiver ==null) return;
    
            var currentNormalizedTime = stateInfo.normalizedTime % 1;

            if (currentNormalizedTime >= castTime)
            {
                RaiseEvent();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_eventsReceiver == null) return;
    
            if (castTime >= 1f)
            {
                RaiseEvent();
            }

            _raised = false;
            _eventsReceiver = null;
        }

        private void RaiseEvent()
        {
            if (_raised) return;
        
            _eventsReceiver.Raise(eventName);
            _raised = true;
        }
    }
}