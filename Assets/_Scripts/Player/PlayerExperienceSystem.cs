using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Spells;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerExperienceSystem : MonoBehaviour, ISaveable<ExperienceData>
    {
        [SerializeField] private ExperienceConfig experienceConfig;
        
        public event Action<ElementType, int, int> OnLevelChanged;
        public event Action<ElementType, float, float> OnExperienceChanged;

        private Dictionary<ElementType, int> _elementTypeToLevel = new();
        private Dictionary<ElementType, float> _elementTypeToExperience = new();
    
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

        public SaveData Save() => new ExperienceData(_elementTypeToLevel, _elementTypeToExperience);

        public void Load(ExperienceData data)
        {
            if (!data.TryGetData(
                    out var elementTypeToLevel,
                    out var elementTypeToExperience)) return;
            
            _elementTypeToLevel = elementTypeToLevel;
            _elementTypeToExperience = elementTypeToExperience;
        }
    }

    public class ExperienceData : SaveData
    {
        [JsonProperty] private Dictionary<string, int> _elementTypeToLevel;
        [JsonProperty] private Dictionary<string, float> _elementTypeToExperience;

        public bool TryGetData(
            out Dictionary<ElementType, int> elementTypeToLevel,
            out Dictionary<ElementType, float> elementTypeToExperience)
        {
            elementTypeToLevel = default;
            elementTypeToExperience = default;

            var spellsManager = SpellsManager.Instance;
            var elementTypes = spellsManager.ElementTypes;
            
            if (spellsManager == null) return false;
            if (elementTypes == null) return false;

            elementTypeToLevel = elementTypes.ConvertToElementTypeDictionary(_elementTypeToLevel);
            elementTypeToExperience = elementTypes.ConvertToElementTypeDictionary(_elementTypeToExperience);

            return elementTypeToLevel != null && elementTypeToExperience != null;
        }

        public ExperienceData()
        {
        }

        public ExperienceData(
            Dictionary<ElementType, int> elementTypeToLevel,
            Dictionary<ElementType, float> elementTypeToExperience)
        {
            _elementTypeToLevel = elementTypeToLevel
                .ToDictionary(x => x.Key.Id, x => x.Value);
            
            _elementTypeToExperience = elementTypeToExperience
                .ToDictionary(x => x.Key.Id, x => x.Value);
        }
    }
}