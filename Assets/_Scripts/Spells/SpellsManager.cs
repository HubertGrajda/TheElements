using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Player;
using _Scripts.Spells;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SpellsManager : Singleton<SpellsManager>, ISaveable<SpellsData>
    {
        [field: SerializeField] public ElementTypes ElementTypes { get; private set; }
        [field: SerializeField] public SpellsConfig Spells { get; private set; }
        
        public Action<ElementType> OnActiveElementChanged;
        public Action<ElementType> OnBendingStyleUnlocked;
        public Action<ElementType, int> OnAvailableSkillPointsChanged;

        public Action<ElementType, SpellConfig> OnActiveSpellChanged;
        public Action<SpellConfig> OnSpellUnlocked;
        public Action<SpellConfig> OnSpellSelected;
        public Action<SpellConfig> OnSpellDeselected;
        
        public List<ElementType> BendingStyles { get; private set; } = new();
        public List<SpellConfig> UnlockedSpells { get; private set; } = new();
        public Dictionary<ElementType, List<SpellConfig>> SelectedSpells { get; private set; } = new();
        private Dictionary<ElementType, int> AvailableSkillPoints { get; set; } = new();

        private void Start()
        {
            if (PlayerManager.Instance.TryGetPlayerComponent(out PlayerExperienceSystem experienceSystem))
            {
                experienceSystem.OnLevelChanged += OnLevelChanged;
            }
        }

        private void OnLevelChanged(ElementType elementType, int prevLevel, int newLevel)
        {
            var earnedSkillPoints = newLevel - prevLevel;

            EarnSkillPoints(elementType, earnedSkillPoints);
            
            if (prevLevel == 0)
            {
                UnlockBendingStyle(elementType);
            }
        }

        private void EarnSkillPoints(ElementType elementType, int skillPoints)
        {
            if (!AvailableSkillPoints.TryAdd(elementType, skillPoints))
            {
                AvailableSkillPoints[elementType] += skillPoints;
            }
            
            OnAvailableSkillPointsChanged?.Invoke(elementType, AvailableSkillPoints[elementType]);
        }
        
        public int GetAvailableSkillPoints(ElementType elementType) => 
            AvailableSkillPoints.GetValueOrDefault(elementType, 0);

        public bool TryToUnlockSpell(SpellConfig spellConfig)
        {
            var elementType = spellConfig.ElementType;
            
            if (GetAvailableSkillPoints(elementType) <= 0) return false;
            
            UnlockSpell(spellConfig);
            
            AvailableSkillPoints[elementType]--;
            OnAvailableSkillPointsChanged?.Invoke(elementType, AvailableSkillPoints[elementType]);
            
            return true;
        }

        private void UnlockSpell(SpellConfig spellConfig)
        {
            if (UnlockedSpells.Contains(spellConfig)) return;
            
            UnlockedSpells.Add(spellConfig);
            OnSpellUnlocked?.Invoke(spellConfig);
        }

        public void SelectSpell(SpellConfig spellConfig)
        {
            if (spellConfig == null) return;
            
            var spellType = spellConfig.ElementType;
            if (SelectedSpells.TryGetValue(spellType, out var selectedSpells))
            {
                selectedSpells.Add(spellConfig);
            }
            else
            {
                SelectedSpells.Add(spellType, new List<SpellConfig> { spellConfig });
            }
            
            OnSpellSelected?.Invoke(spellConfig);
        }

        public void DeselectSpell(SpellConfig spellConfig)
        {
            if (spellConfig == null) return;
            if (!SelectedSpells.TryGetValue(spellConfig.ElementType, out var selectedSpells)) return;
            if (!selectedSpells.Contains(spellConfig)) return;
            
            selectedSpells.Remove(spellConfig);
            
            OnSpellDeselected?.Invoke(spellConfig);
        }

        public void UnlockBendingStyle(ElementType elementType)
        {
            if (BendingStyles.Contains(elementType)) return;
            
            BendingStyles.Add(elementType);
            OnBendingStyleUnlocked?.Invoke(elementType);
            
            UnlockSpell(elementType.InitialSpell);
            SelectSpell(elementType.InitialSpell);
        }

        public bool IsSpellSelected(SpellConfig spellConfig) =>
            SelectedSpells.TryGetValue(spellConfig.ElementType, out var selectedSpells) && 
            selectedSpells.Contains(spellConfig);

        public SaveData Save()
        {
            var unlockedSpells = UnlockedSpells.Select(x => x.name).ToList();
            var unlockedBendingStyles = BendingStyles.Select(x => x.Id).ToList();
            var availableSkillPoints = AvailableSkillPoints.ToDictionary(x => x.Key.Id, x => x.Value);
            var selectedSpells = SelectedSpells.ToDictionary(
                x => x.Key.Id,
                x => x.Value.Select(y => y.name).ToList());
            
            return new SpellsData(unlockedSpells, unlockedBendingStyles, availableSkillPoints, selectedSpells);
        }

        public void Load(SpellsData data)
        {
            if (!data.TryGetData(
                    out var unlockedSpells,
                    out var unlockedBendingStyles,
                    out var availableSkillPoints,
                    out var selectedSpells)) return;
            
            UnlockedSpells = unlockedSpells;
            BendingStyles = unlockedBendingStyles;
            AvailableSkillPoints = availableSkillPoints;
            SelectedSpells = selectedSpells;
        }
    }

    [Serializable]
    public class SpellsData : SaveData
    {
        [JsonProperty] private List<string> _unlockedSpells;
        [JsonProperty] private List<string> _unlockedBendingStyles;
        [JsonProperty] private Dictionary<string, int> _availableSkillPoints;
        [JsonProperty] private Dictionary<string, List<string>> _selectedSpells;
            
        public SpellsData()
        {
        }
        
        public SpellsData(
            List<string> unlockedSpells,
            List<string> unlockedBendingStyles,
            Dictionary<string, int> availableSkillPoints,
            Dictionary<string, List<string>> selectedSpells)
        {
            _unlockedSpells = unlockedSpells;
            _unlockedBendingStyles = unlockedBendingStyles;
            _availableSkillPoints = availableSkillPoints;
            _selectedSpells = selectedSpells;
        }

        public bool TryGetData(
            out List<SpellConfig> unlockedSpells,
            out List<ElementType> unlockedBendingStyles,
            out Dictionary<ElementType, int> availableSkillPoints,
            out Dictionary<ElementType, List<SpellConfig>> selectedSpells)
        {
            unlockedSpells = default;
            unlockedBendingStyles = default;
            availableSkillPoints = default;
            selectedSpells = default;
            
            var spellsManager = SpellsManager.Instance;
            
            if (spellsManager == null) return false;
            if (spellsManager.Spells == null) return false;
            if (spellsManager.ElementTypes == null) return false;

            unlockedSpells = spellsManager.Spells.ConvertToListOfSpellConfigs(_unlockedSpells);
            unlockedBendingStyles = spellsManager.ElementTypes.ConvertToListOfElementTypes(_unlockedBendingStyles);
            availableSkillPoints = spellsManager.ElementTypes.ConvertToElementTypeDictionary(_availableSkillPoints);
            
            var selectedSpellsNames = spellsManager.ElementTypes.ConvertToElementTypeDictionary(_selectedSpells);
            selectedSpells = selectedSpellsNames.ToDictionary(
                x => x.Key,
                x => spellsManager.Spells.ConvertToListOfSpellConfigs(x.Value));
            
            if (unlockedSpells == null) return false;
            if (unlockedBendingStyles == null) return false;
            if (unlockedBendingStyles == null) return false;
            if (selectedSpells == null) return false;

            return true;
        }
    }
}