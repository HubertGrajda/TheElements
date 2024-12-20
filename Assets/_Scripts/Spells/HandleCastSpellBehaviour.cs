using UnityEngine;

namespace _Scripts.Spells
{
    public class HandleCastSpellBehaviour : StateMachineBehaviour
    {
        [Tooltip("0 - casted on start, 1 - casted on exit")]
        [SerializeField, Range(0f,1f)] private float castTime;
    
        [Tooltip("0 - launched on start, 1 - launched on exit")]
        [SerializeField, Range(0f,1f)] private float launchTime;

        private bool _launched;
        private bool _casted;
        private SpellLauncher _spellLauncher;
    
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.TryGetComponent(out _spellLauncher)) return;
        
            if (castTime <= 0f)
            {
                _spellLauncher.CastSpell();
                _casted = true;
            }

            if (launchTime <= 0f)
            {
                _spellLauncher.LaunchSpell();
                _launched = true;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_spellLauncher == null) return;
        
            var currentNormalizedTime = stateInfo.normalizedTime % 1;

            if (!_casted && currentNormalizedTime >= castTime)
            {
                _spellLauncher.CastSpell();
                _casted = true;
            }

            if (!_launched && currentNormalizedTime >= launchTime)
            {
                _spellLauncher.LaunchSpell();
                _launched = true;
            }
        }
    
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_spellLauncher == null) return;
        
            if (!_casted && castTime >= 1f)
            {
                _spellLauncher.CastSpell();
                _casted = true;
            }

            if (!_launched && launchTime >= 1f)
            {
                _spellLauncher.LaunchSpell();
            }

            _casted = false;
            _launched = false;
            _spellLauncher = null;
        }
    }
}