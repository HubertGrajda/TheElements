using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Instruction3D : MonoBehaviour
{
    [TextArea]
    [SerializeField] private List<string> listOfInstructions;
    [SerializeField] private float rotationSpeed = 10;
    
    [SerializeField] private GameObject textContainer;
    [SerializeField] private TMP_Text instruction;
    
    private InputAction _inputAction;
    
    private int _currInstructionIndex;
    private bool _isShown;
    
    private const float SHOW_ANIMATION_DURATION = 2f;
    
    private void Start()
    {
        instruction.text = listOfInstructions[0];
        _inputAction = InputsManager.Instance.PlayerActions.Accept;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        if (!_isShown)
        {
            textContainer.transform.DOScale(Vector3.one, SHOW_ANIMATION_DURATION);
            _isShown = true;
        }

        _inputAction.started += NextInstruction;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        
        RotateToPlayer(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Constants.Tags.PLAYER_TAG)) return;
        if (!_isShown) return;

        _inputAction.started -= NextInstruction; 
        textContainer.transform.DOScale(Vector3.zero, SHOW_ANIMATION_DURATION);
        _isShown = false;
    }
    
    private void NextInstruction(InputAction.CallbackContext context)
    {
        instruction.text = listOfInstructions[++_currInstructionIndex % listOfInstructions.Count];
    }
    
    private void RotateToPlayer(Transform playerTransform)
    {
        var rotation = Quaternion.LookRotation(transform.position - playerTransform.transform.position);
        rotation.x = 0f;
        rotation.z = 0f;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    
    private void OnDestroy()
    {
        if (_inputAction == null) return;
        
        _inputAction.started -= NextInstruction; 
    }
}
