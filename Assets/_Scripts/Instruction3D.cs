using System;
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

    private int _currInstructionIndex;
    private Transform _playerTransform;
    private bool _isShown;
    
    private void Start()
    {
        instruction.text = listOfInstructions[0];
        _playerTransform = Managers.PlayerManager.PlayerRef.transform; 
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

            Managers.InputManager.PlayerActions.Accept.started += NextInstruction;
        }
        else
        {
            if(!_isShown) return;

            Managers.InputManager.PlayerActions.Accept.started -= NextInstruction; 
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
        Managers.InputManager.PlayerActions.Accept.started -= NextInstruction; 
    }
}