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
    [SerializeField] private float activationDistance = 3.5f;
    [SerializeField] private GameObject textContainer;
    [SerializeField] private TMP_Text instruction;

    private Transform _playerTransform;
    private InputAction _inputAction;
    
    private int _currInstructionIndex;
    private bool _isShown;
    
    private void Start()
    {
        instruction.text = listOfInstructions[0];
        _playerTransform = PlayerManager.Instance.PlayerRef.transform;
        _inputAction = InputsManager.Instance.PlayerActions.Accept;
    }
    
    private void Update()
    {
        if(_playerTransform == null) return;
        
        InstructionHandling();
        
        if(!_isShown) return;
        
        RotateToPlayer();
    }

    private void InstructionHandling()
    {
        var distance = Vector3.Distance(_playerTransform.transform.position, transform.position);
        
        if (distance < activationDistance)
        {
            if (!_isShown)
            {
                textContainer.transform.DOScale(new Vector3(1f, 1f, 1f), 2f);
                _isShown = true;
            }

            _inputAction.started += NextInstruction;
        }
        else
        {
            if(!_isShown) return;

            _inputAction.started -= NextInstruction; 
            textContainer.transform.DOScale(new Vector3(0, 0, 0), 2f);
            _isShown = false;
        }
    }
    
    private void NextInstruction(InputAction.CallbackContext context)
    {
        instruction.text = listOfInstructions[_currInstructionIndex++ % listOfInstructions.Count];
    }
    
    private void RotateToPlayer()
    {
        var rotation = Quaternion.LookRotation(transform.position - _playerTransform.transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation,new Quaternion(0, rotation.y, 0, rotation.w), 0.01f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }

    private void OnDestroy()
    {
        if (_inputAction == null) return;
        
        _inputAction.started -= NextInstruction; 
    }
}
