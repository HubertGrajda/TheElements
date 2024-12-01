using UnityEngine;

namespace _Scripts.Spells
{
    public interface ISpell
    {
        public void Use(Animator animator);
        public void Cast();
        public void PrepareToLaunch();
        public void Launch();
        public void Cancel();
    }
}