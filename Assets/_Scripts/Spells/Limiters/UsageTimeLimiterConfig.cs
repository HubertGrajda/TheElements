﻿using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "New UsageTimeLimiter", menuName = "Spells/Limiters/UsageTimeLimiter")]
    public class UsageTimeLimiterConfig : SpellLimiterConfig<UsageTimeLimiter>
    {
        [SerializeField] private float maxDuration;

        protected override UsageTimeLimiter CreateInstance(SpellLimiterController controller, Spell spell)
        {
            return new UsageTimeLimiter(controller, maxDuration);
        }
    }
}