using System.Collections;

namespace _Scripts.Spells
{
    public class LongSpell : Spell
    {
        private bool CanBePerformed => Launched && !Cancelled;
        public override bool CanBeLaunched => !Cancelled;
        
        public override void Launch()
        {
            base.Launch();
            StartCoroutine(StartPerforming());
        }

        private IEnumerator StartPerforming()
        {
            while (CanBePerformed)
            {
                Perform();
                yield return null;
            }
        }
        
        protected virtual void Perform()
        {
        }

        public override void Cancel()
        {
            if (Cancelled) return;
            
            base.Cancel();
            SpellCollider.enabled = false;
        }
    }
}