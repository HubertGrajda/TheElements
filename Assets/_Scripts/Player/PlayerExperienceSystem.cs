using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerExperienceSystem : MonoBehaviour
    {
        [SerializeField] private ExperienceConfig experienceConfig;
        
        public event Action<ElementType, int, int> OnLevelChanged;
        public event Action<ElementType, float, float> OnExperienceChanged;

        private readonly Dictionary<ElementType, int> _elementTypeToLevel = new();
        private readonly Dictionary<ElementType, float> _elementTypeToExperience = new();
    
        public void AddExperience(ElementType elementType, float value)
        {
            if (!elementType) return;
        
            
            if (!_elementTypeToExperience.TryAdd(elementType, value))
            {
                _elementTypeToExperience[elementType] += value;
            }
            
            OnExperienceChanged?.Invoke(elementType, _elementTypeToExperience[elementType], value);

            TryReachNextLevel(elementType);
        }

        public float GetCurrentExperience(ElementType elementType) =>
            _elementTypeToExperience.GetValueOrDefault(elementType, 0f);

        public int GetCurrentLevel(ElementType elementType) =>
            _elementTypeToLevel.GetValueOrDefault(elementType, 0);

        public float GetRequiredExperience(int level) =>
            experienceConfig.TryGetRequiredExperienceForLevel(level, out var requiredExperience)
                ? requiredExperience
                : 0f;
        
        private void TryReachNextLevel(ElementType elementType)
        {
            var currentExperience = _elementTypeToExperience[elementType];
            
            if (!experienceConfig.TryGetLevelForExperience(currentExperience, out var level)) return;

            var currentLevel = _elementTypeToLevel.GetValueOrDefault(elementType, 0);
            
            if (level == currentLevel) return;
            
            _elementTypeToLevel[elementType] = level;
            OnLevelChanged?.Invoke(elementType, currentLevel, level);
        }
    }
}