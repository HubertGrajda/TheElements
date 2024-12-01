using _Scripts.Managers;

namespace _Scripts.Spells
{
    public abstract class SpellLimiterConfig<TSpellLimiter> : SpellLimiterConfigBase where TSpellLimiter : SpellLimiter
    {
        public override SpellLimiter GetInstance(Spell spell)
        {
            var controller = SpellsManager.Instance.SpellLimiterController;
            
            if (!controller.TryGetLimiter(spell.SpellData, out var limiter))
            {
                limiter = CreateInstance(controller, spell);
                controller.AddLimiter(spell.SpellData, limiter);
            }

            return limiter;
        }

        protected abstract TSpellLimiter CreateInstance(SpellLimiterController controller, Spell spell);
    }
}