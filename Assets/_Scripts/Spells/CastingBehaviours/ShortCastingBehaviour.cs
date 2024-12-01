using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "Short casting behaviour", menuName = "Spells/CastingBehaviour/Short")]
    public class ShortCastingBehaviour : SpellCastingBehaviour
    {
        public override void StartCasting(Animator casterAnimator)
        {
            casterAnimator.SetTrigger(CastingAnimationHash);
        }
        
        public override void StopCasting(Animator casterAnimator)
        {
        }
    }
}