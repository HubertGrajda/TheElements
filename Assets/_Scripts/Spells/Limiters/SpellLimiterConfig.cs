namespace _Scripts.Spells
{
    public abstract class SpellLimiterConfig<TSpellLimiter> : SpellLimiterConfigBase where TSpellLimiter : SpellLimiter
    {
        public override SpellLimiter CreateLimiterInstance() => CreateInstance();

        protected abstract TSpellLimiter CreateInstance();
    }
}