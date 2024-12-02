using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellCastingBehaviour : ScriptableObject
    {
        [SerializeField] private string castingAnimationName;
        protected int CastingAnimationHash => Animator.StringToHash(castingAnimationName);

        public abstract void StartCasting(Animator casterAnimator);
        
        public abstract void StopCasting(Animator casterAnimator);
    }
}