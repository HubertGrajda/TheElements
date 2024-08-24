using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
    private Camera _cameraUI;
    
    private void Awake()
    {
        _cameraUI = GetComponent<Camera>();
    }

    private void Start()
    {
        Managers.CamerasManager.SetUICamera(_cameraUI);
    }
}
