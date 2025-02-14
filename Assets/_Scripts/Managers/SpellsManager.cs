using System;
using System.Collections.Generic;
using _Scripts.Player;
using _Scripts.Spells;

namespace _Scripts.Managers
{
    public class SpellsManager : Singleton<SpellsManager>
    {

        public Action<ElementType> OnActiveElementChanged;
        public Action<ElementType> OnBendingStyleUnlocked;
        public Action<ElementType, int> OnAvailableSkillPointsChanged;

        public Action<ElementType, SpellConfig> OnActiveSpellChanged;
        public Action<SpellConfig> OnSpellUnlocked;
        public Action<SpellConfig> OnSpellSelected;
        public Action<SpellConfig> OnSpellDeselected;
        
        public List<ElementType> BendingStyles { get; } = new();
        public List<SpellConfig> UnlockedSpells { get; } = new();
        public Dictionary<ElementType, List<SpellConfig>> SelectedSpells { get; } = new();
        private Dictionary<ElementType, int> AvailableSkillPoints { get; } = new();

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
    }
}