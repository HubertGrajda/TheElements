using UnityEngine;

namespace _Scripts.AI
{
    public class SpellAttackAIState : AIState
    {
        protected override bool CanBeEntered =>
            HasTarget &&
            DistanceToTarget > Stats.RangedAttackMinDistance &&
            DistanceToTarget < Stats.RangedAttackMaxDistance;

        protected override bool CanBeEnded => !CanBeEntered && !_spellLauncher.IsLaunching;

        private float _timer;
        private readonly AISpellLauncher _spellLauncher;
    
        public SpellAttackAIState(AIStateMachine fsm) : base(fsm)
        {
            _spellLauncher = fsm.SpellLauncher;
        }
    
        public override void UpdateState()
        {
            base.UpdateState();
        
            Fsm.transform.LookAt(Fsm.TargetTransform);
        
            if (_timer < 0)
            {
                _spellLauncher.UseSpell();
                _timer = Stats.ShootingFrequency;
            }
        
            _timer -= Time.deltaTime;
        }
    }
}