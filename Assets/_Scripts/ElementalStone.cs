using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ElementalStone : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp expBall;
    [SerializeField] private Animator anim;

    private bool _alreadyUsed;
    private GameObject _player;
    private InputAction _inputAction;
    
    private static readonly int InRangeAnimParam = Animator.StringToHash("IsActive");

    private void Start()
    {
        _inputAction = InputManager.Instance.PlayerActions.Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyUsed || !other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
        _inputAction.started += InteractionBehaviour;
        _player = other.gameObject;
        anim.SetBool(InRangeAnimParam, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_alreadyUsed || !other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
        anim.SetBool(InRangeAnimParam, false);
    }

    public void InteractionBehaviour(InputAction.CallbackContext context)
    {
        _inputAction.started -= InteractionBehaviour;
        _alreadyUsed = true;
        expBall.CollectPowerUp(_player);
        anim.SetBool(InRangeAnimParam, false);
    }
}
