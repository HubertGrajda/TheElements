using System.Linq;
using UnityEngine;

namespace _Scripts.Spells
{
    public abstract class SpellCastingBehaviour : ScriptableObject
    {
        [SerializeField] private string castingAnimationName;
        
        private int CastingAnimationHash => Animator.StringToHash(castingAnimationName);

        public void ToggleCastingAnimation(SpellLauncher spellLauncher, bool isCasting)
        {
            var animator = spellLauncher.Animator;
            var param = animator.parameters
                .FirstOrDefault(x => x.nameHash == CastingAnimationHash);
            
            if (param == null) return;

            switch (param.type)
            {
                case AnimatorControllerParameterType.Bool:
                {
                    animator.SetBool(CastingAnimationHash, isCasting);
                    return;
                }
                case AnimatorControllerParameterType.Trigger:
                {
                    if (isCasting)
                    {
                        animator.SetTrigger(CastingAnimationHash);
                    }
                    
                    return;
                }
            }
        }
    }
}