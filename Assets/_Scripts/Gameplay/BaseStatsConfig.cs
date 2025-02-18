using UnityEngine;

namespace _Scripts
{
    public abstract class BaseStatsConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
    }
}