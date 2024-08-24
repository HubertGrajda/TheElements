using UnityEngine;
using TMPro;

public class AbilitiesMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text selectedAbilityName;
    [SerializeField] private TMP_Text selectedAbilityDescription;
   
    public void SetAbilityInfo(string name, string descriptiion)
    {
        selectedAbilityName.text = name;
        selectedAbilityDescription.text = descriptiion;
    }
}
