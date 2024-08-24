using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellCastingBehaviour : ScriptableObject
    {
        [field: SerializeField] public string CastingAnimationName {get; private set;}
        [field: SerializeField] public AudioClip CastingSound {get; private set;}
        
        public abstract void CasterStartCasting(Animator casterAnimator);
        public abstract void CasterStopCasting(Animator casterAnimator);
    }
}