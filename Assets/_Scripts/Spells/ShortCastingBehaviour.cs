using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "Short casting behaviour", menuName = "Spells/CastingBehaviour/Short")]
    public class ShortCastingBehaviour : SpellCastingBehaviour
    {
        public override void CasterStartCasting(Animator casterAnimator)
        {
            casterAnimator.SetTrigger(CastingAnimationName);
        }
        
        public override void CasterStopCasting(Animator casterAnimator)
        {
        }
    }
}