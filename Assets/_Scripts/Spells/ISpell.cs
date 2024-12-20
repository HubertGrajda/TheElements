namespace _Scripts.Spells
{
    public interface ISpell
    {
        public void Use(SpellLauncher spellLauncher);
        public void Cast();
        public void PrepareToLaunch();
        public void Launch();
        public void Cancel();
    }
}