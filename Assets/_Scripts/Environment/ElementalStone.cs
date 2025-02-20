using _Scripts.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts
{
    [RequireComponent(typeof(Collider))]
    public class ElementalStone : MonoBehaviour, IInputInteractable
    {
        [SerializeField] private PowerUp expBall; //TODO: restrict to interface attribute
        [SerializeField] private Animator anim;

        private bool _alreadyUsed;
        private InputAction _inputAction;
    
        private static readonly int InRangeAnimParam = Animator.StringToHash("IsActive");

        private void Start()
        {
            _inputAction = InputsManager.Instance.PlayerActions.Interact;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_alreadyUsed) return;
            if (!other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
            _inputAction.started += InteractionBehaviour;
            anim.SetBool(InRangeAnimParam, true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_alreadyUsed) return;
            if (!other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
            anim.SetBool(InRangeAnimParam, false);
        }

        public void InteractionBehaviour(InputAction.CallbackContext context)
        {
            _inputAction.started -= InteractionBehaviour;
            _alreadyUsed = true;
            expBall.Collect();
            anim.SetBool(InRangeAnimParam, false);
        }
    }
}