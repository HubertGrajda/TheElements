
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        private PlayerController _playerController;
        
        private PlayerController PlayerController => _playerController != null 
            ? _playerController 
            : _playerController = FindAnyObjectByType<PlayerController>();
        
        private readonly Dictionary<Type, Component> _playerComponents = new();

        public void Clear()
        {
            _playerComponents.Clear();
            _playerController = null;
        }
        
        public void SetUpPlayerComponent(PlayerController player)
        {
            _playerController = player;
        }

        public bool TryGetPlayerComponent<T>(out T component) where T : Component
        {
            if (_playerComponents.TryGetValue(typeof(T), out var matchingComponent))
            {
                component = matchingComponent as T;
                return true;
            }

            component = default;
            
            if (PlayerController == null) return false;

            if (_playerController.TryGetComponent(out component))
            {
                _playerComponents.Add(typeof(T), component);
            }

            return component != null;
        }
    }
}