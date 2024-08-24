using UnityEngine;

[CreateAssetMenu( fileName = "newAbilityUI" , menuName = "abilityUI" )]
public class Ability_SO : ScriptableObject
{
    public Sprite border;
    public Sprite icon;
    public bool unlocked;
    public Color lockedIconColor;
    public Color unlockedIconColor;
    public string abilityName;
    public string abilityDescription;

}
