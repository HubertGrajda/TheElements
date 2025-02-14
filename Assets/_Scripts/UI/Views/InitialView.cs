using _Scripts.Managers;

namespace _Scripts.UI
{
    public class InitialView : View
    {
        private SpellsManager _spellsManager;
        
        private void Start()
        {
            _spellsManager = SpellsManager.Instance;

            if (_spellsManager.UnlockedSpells.Count != 0) return;
            
            Show();
            _spellsManager.OnBendingStyleUnlocked += OnBendingStyleUnlocked;
        }

        private void OnBendingStyleUnlocked(ElementType elementType)
        {
            _spellsManager.OnBendingStyleUnlocked -= OnBendingStyleUnlocked;
            Hide();
        }
    }
}