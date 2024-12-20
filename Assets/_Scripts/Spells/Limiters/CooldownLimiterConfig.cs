using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "New CooldownLimiter", menuName = "Spells/Limiters/CooldownLimiter")]
    public class CooldownLimiterConfig : SpellLimiterConfig<CooldownLimiter>
    {
        [SerializeField] private float cooldown;

        protected override CooldownLimiter CreateInstance()
        {
            return new CooldownLimiter(cooldown);
        }
    }
}