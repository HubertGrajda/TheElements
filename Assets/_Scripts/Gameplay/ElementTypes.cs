using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "new elementTypes", menuName = "ElementTypes")]
    public class ElementTypes : ScriptableObject
    {
        [SerializeField] private List<ElementType> elementTypes;

        public bool TryGetElementTypeById(string id, out ElementType elementType) => 
            elementType = elementTypes.FirstOrDefault(x => x.Id == id);
        
        public Dictionary<ElementType, T> ConvertToElementTypeDictionary<T>(Dictionary<string, T> source)
        {
            var result = new Dictionary<ElementType, T>();

            foreach (var kvp in source)
            {
                if (!TryGetElementTypeById(kvp.Key, out var elementType)) continue;
                
                result[elementType] = kvp.Value;
            }

            return result;
        }
        
        public List<ElementType> ConvertToListOfElementTypes(List<string> source)
        {
            var result = new List<ElementType>();

            foreach (var name in source)
            {
                var elementType = elementTypes.FirstOrDefault(x => x.name == name);
                
                if (elementType == null) continue;
                
                result.Add(elementType);
            }

            return result;
        }
    }
}