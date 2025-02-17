using _Scripts.Spells;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "new elementType", menuName = "ElementType")]
    public class ElementType : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public SpellConfig InitialSpell { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
    }
}