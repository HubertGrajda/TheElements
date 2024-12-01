using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "New CooldownLimiter", menuName = "Spells/Limiters/CooldownLimiter")]
    public class CooldownLimiterConfig : SpellLimiterConfig<CooldownLimiter>
    {
        [SerializeField] private float cooldown;

        protected override CooldownLimiter CreateInstance(SpellLimiterController controller, Spell spell)
        {
            return new CooldownLimiter(controller, cooldown);
        }
    }
}