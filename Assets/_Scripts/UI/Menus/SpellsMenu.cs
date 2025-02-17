using _Scripts.Player;
using _Scripts.Spells;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class SpellsMenu : Menu
    {
        [field: SerializeField] public ElementType ElementType { get; private set; }

        [SerializeField] private TMP_Text skillPointsText;
        [SerializeField] private TMP_Text currentLevelText;
        
        private SpellsManager _spellsManager;
        private PlayerManager _playerManager;
        private PlayerExperienceSystem _playerExperienceSystem;

        protected override void Start()
        {
            _spellsManager = SpellsManager.Instance;
            _playerManager = PlayerManager.Instance;
            
            if (_playerManager.TryGetPlayerComponent(out _playerExperienceSystem))
            {
                _playerExperienceSystem.OnLevelChanged += OnLevelChanged;
            }
            _spellsManager.OnAvailableSkillPointsChanged += OnAvailableSkillPointsChanged;
            
            base.Start();
        }

        protected override void InitUIElements()
        {
            InitCurrentLevelText();
            InitAvailableSkillPointsText();
        }

        private void OnDestroy()
        {
            _spellsManager.OnAvailableSkillPointsChanged -= OnAvailableSkillPointsChanged;
        }

        private void InitCurrentLevelText()
        {
            if (!currentLevelText) return;
            if (!_playerExperienceSystem) return;
            
            currentLevelText.text = _playerExperienceSystem.GetCurrentLevel(ElementType).ToString();
        }
        
        private void OnLevelChanged(ElementType element, int previousLevel, int currentLevel)
        {
            if (element != ElementType) return;
            
            if (currentLevelText)
            {
                currentLevelText.text = currentLevel.ToString();
            }
        }
        
        private void OnAvailableSkillPointsChanged(ElementType element, int value)
        {
            if (!skillPointsText) return;
            if (element != ElementType) return;
            
            skillPointsText.text = value.ToString();
        }

        private void InitAvailableSkillPointsText()
        {
            if (!skillPointsText) return;
            
            skillPointsText.text = _spellsManager.GetAvailableSkillPoints(ElementType).ToString();
        }
    }
}