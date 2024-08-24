using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "new spell data", menuName = "Spells/SpellData")]
    public class SpellDataSO : ScriptableObject
    {
        [field: SerializeField] public bool IsChildOfSpawnPoint { get; private set;}
        [field: SerializeField] public float Cooldown { get; private set;}
        
        [field: SerializeField] public SpellCastingBehaviour CastingBehaviour { get; private set;}
        
        [field: SerializeField] public ElementType ElementType { get; private set;}
    }
    
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public abstract class SpellLaunchingBehaviour : ScriptableObject
    {
        [field: SerializeField] public bool IsChildOfSpawnPoint {get; private set;}
        
    }
    
}