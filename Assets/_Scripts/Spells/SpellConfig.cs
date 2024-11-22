using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "new spell config", menuName = "Spells/SpellConfig")]
    public class SpellConfig : ScriptableObject
    {
        [field: SerializeField] public bool IsChildOfSpawnPoint { get; private set;}
        [field: SerializeField] public float Cooldown { get; private set;}
        [field: SerializeField] public SpellCastingBehaviour CastingBehaviour { get; private set;}
        [field: SerializeField] public ElementType ElementType { get; private set;}
        [field: SerializeField] public Spell SpellPrefab { get; private set;}
    }
    
    public abstract class SpellLaunchingBehaviour : ScriptableObject
    {
        [field: SerializeField] public bool IsChildOfSpawnPoint {get; private set;}
        
    }
}