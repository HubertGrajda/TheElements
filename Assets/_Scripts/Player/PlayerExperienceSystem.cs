using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperienceSystem : MonoBehaviour
{
    public Action<ElementType, float> OnExperienceChanged;

    private readonly Dictionary<ElementType, float> _elementTypeToExperience = new();
    
    public void AddExperience(ElementType elementType, float value)
    {
        if (elementType == null) return;
        
        if (!_elementTypeToExperience.TryAdd(elementType, value))
        {
            _elementTypeToExperience[elementType] += value;
        }

        OnExperienceChanged?.Invoke(elementType, _elementTypeToExperience[elementType]);
    }

    public float GetExperienceValue(ElementType elementType) =>
        _elementTypeToExperience.GetValueOrDefault(elementType, 0f);
}