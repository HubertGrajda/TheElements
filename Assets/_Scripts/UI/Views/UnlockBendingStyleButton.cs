using _Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class UnlockBendingStyleButton : ButtonAction
    {
        [SerializeField] private ElementType typeToUnlock;
        
        private SpellsManager _spellsManager;
        private Image _image;

        protected override bool IsValid => typeToUnlock != null && _image != null;
        
        protected override void OnClick()
        {
            _spellsManager.UnlockBendingStyle(typeToUnlock);
        }

        protected override void Prepare()
        {
            _spellsManager = SpellsManager.Instance;
            
            _image = Button.GetComponent<Image>();
            _image.sprite = typeToUnlock.Icon;
        }
    }
}