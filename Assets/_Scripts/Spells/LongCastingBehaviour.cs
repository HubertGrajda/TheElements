using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "Long casting behaviour", menuName = "Spells/CastingBehaviour/Long")]
    public class LongCastingBehaviour : SpellCastingBehaviour
    {
        public override void CasterStartCasting(Animator casterAnimator)
        {
            casterAnimator.SetBool(CastingAnimationName, true);
        }
        
        public override void CasterStopCasting(Animator casterAnimator)
        {
            casterAnimator.SetBool(CastingAnimationName, false);
        }
    }
}