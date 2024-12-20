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
    
        protected void Start()
        {
            PlayerManager.Instance.TryGetPlayerComponent(out _playerController);
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