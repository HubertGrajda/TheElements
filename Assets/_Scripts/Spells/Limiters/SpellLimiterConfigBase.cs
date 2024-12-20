using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellLimiterConfigBase : ScriptableObject
    {
        public abstract SpellLimiter CreateLimiterInstance();
    }
}