using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDisplay : MonoBehaviour
{
    private AbilitiesMenu _abilitiesMenu;

    [SerializeField] private Ability_SO ability;
    [SerializeField] private Image border;
    [SerializeField] private Image icon;
    [SerializeField] private bool unlocked;

    private void Start()
    {
        _abilitiesMenu = GetComponentInParent<AbilitiesMenu>();
        border.sprite = ability.border;
        icon.sprite = ability.icon;
        unlocked = ability.unlocked;

        if (!unlocked)
            icon.color = ability.lockedIconColor;
    }

    public void SetAbilityInfo()
    {
        _abilitiesMenu.SetAbilityInfo(ability.abilityName, ability.abilityDescription);
    }
    
    public void UnlockAbility()
    {
        ability.unlocked = true;
        icon.color = ability.unlockedIconColor;
    }
}
