using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ElementalStone : MonoBehaviour, IInteractable
{
    [SerializeField] private PowerUp expBall;
    [SerializeField] private Animator anim;

    private bool _alreadyUsed;
    private GameObject _player;
    
    private static readonly int InRangeAnimParam = Animator.StringToHash("IsActive");

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyUsed || !other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
        Managers.InputManager.PlayerActions.Interact.started += InteractionBehaviour;
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
        Managers.InputManager.PlayerActions.Interact.started -= InteractionBehaviour;
        _alreadyUsed = true;
        expBall.CollectPowerUp(_player);
        anim.SetBool(InRangeAnimParam, false);
    }
}
