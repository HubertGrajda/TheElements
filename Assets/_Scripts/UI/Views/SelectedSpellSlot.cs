using _Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class SelectedSpellSlot : MonoBehaviour
    {
        public SpellConfig Spell { get; private set; }
        
        [SerializeField] private Image spellImage;
        [SerializeField] private Image emptyImage;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!Spell) return;
            
            SpellsManager.Instance.DeselectSpell(Spell);
        }

        public void AssignSpell(SpellConfig spell)
        {
            Spell = spell;
            spellImage.sprite = spell.SpellUIConfig.SpellSprite;
            spellImage.gameObject.SetActive(true);
            emptyImage.gameObject.SetActive(false);
        }

        public void UnassignSpell()
        {
            Spell = null;
            spellImage.gameObject.SetActive(false);
            emptyImage.gameObject.SetActive(true);
        }
    }
}