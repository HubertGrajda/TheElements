using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceSystem : MonoBehaviour // TODO: Refactor
{
    [Serializable]
    public class ElementTypeSlider
    {
        [SerializeField] private Slider slider;
        
        [SerializeField] private ElementType elementType;
        public ElementType ElementType => elementType;

        public float Experience { get; private set; }

        public void AddExp(float value)
        {
            Experience += value;
            slider.value = Experience;
        }
    }

    [SerializeField] private List<ElementTypeSlider> sliders;
    
    public void AddExperience(float value, ElementType elementType)
    {
        var elementTypeSlider = sliders.FirstOrDefault(x => x.ElementType == elementType);
        if (elementTypeSlider == null) return;
        
        elementTypeSlider.AddExp(value);
    }

    public bool TryGetExperienceValue(ElementType elementType, out float experience)
    {
        experience = default;
        
        var elementTypeSlider = sliders.FirstOrDefault(x => x.ElementType == elementType);
        if (elementTypeSlider == null) return false;
        
        experience = elementTypeSlider.Experience;
        return true;
    }
    
}