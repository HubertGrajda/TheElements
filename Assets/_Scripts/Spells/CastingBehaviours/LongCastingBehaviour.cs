using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "Long casting behaviour", menuName = "Spells/CastingBehaviour/Long")]
    public class LongCastingBehaviour : SpellCastingBehaviour
    {
        public override void StartCasting(Animator casterAnimator)
        {
            if (casterAnimator == null) return;
            
            casterAnimator.SetBool(CastingAnimationHash, true);
        }
        
        public override void StopCasting(Animator casterAnimator)
        {
            if (casterAnimator == null) return;
            
            casterAnimator.SetBool(CastingAnimationHash, false);
        }
    }
}