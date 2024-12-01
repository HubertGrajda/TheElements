using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Spells
{
    public class SpellLimiterController : MonoBehaviour
    {
        private readonly Dictionary<SpellConfig, SpellLimiter> _spellConfigsToLimiters = new();

        public bool TryGetLimiter(SpellConfig config, out SpellLimiter limiter) =>
            _spellConfigsToLimiters.TryGetValue(config, out limiter);

        public void AddLimiter(SpellConfig config, SpellLimiter limiter) => 
            _spellConfigsToLimiters.TryAdd(config, limiter);
    }
}