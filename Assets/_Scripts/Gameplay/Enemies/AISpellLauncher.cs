using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Spells;
using UnityEngine;

namespace _Scripts.AI
{
    public class AISpellLauncher : SpellLauncher
    {
        [SerializeField] private SpellConfig rangeAttackSpell;

        private PlayerController _playerController;
        private HealthSystem _healthSystem;
        
        protected void Start()
        {
            PlayerManager.Instance.TryGetPlayerComponent(out _playerController);
            if (TryGetComponent(out _healthSystem))
            {
                _healthSystem.OnDamaged += OnDamaged;
            }
        }

        private void OnDamaged(int _)
        {
            if (IsLaunching)
            {
                CancelSpell();
            }
        }

        protected override bool TryGetSpellToUse(out SpellConfig spellConfig)
        {
            spellConfig = rangeAttackSpell;
            return spellConfig != null;
        }

        public override Vector3 GetTarget() => _playerController != null 
            ? _playerController.transform.position 
            : default;
    }
}