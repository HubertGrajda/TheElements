using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "new UIConfig", menuName = "Spells/Config/UIConfig")]
    public class SpellUIConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite SpellSprite { get; private set;}
    }
}