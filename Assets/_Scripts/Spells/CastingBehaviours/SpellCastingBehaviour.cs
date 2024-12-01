using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellCastingBehaviour : ScriptableObject
    {
        [SerializeField] private string castingAnimationName;
        [field: SerializeField] public AudioClip CastingSound {get; private set;}
        
        protected int CastingAnimationHash => Animator.StringToHash(castingAnimationName);

        public virtual void StartCasting(Animator casterAnimator)
        {
            AudioManager.Instance.PlaySound(CastingSound);
        }
        
        public abstract void StopCasting(Animator casterAnimator);
    }
}