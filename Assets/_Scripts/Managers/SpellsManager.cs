using System;
using _Scripts.Spells;

namespace _Scripts.Managers
{
    public class SpellsManager : Singleton<SpellsManager>
    {
        public Action<SpellConfig> OnSelectedSpellChanged;
        public Action<ElementType> OnSelectedElementChanged;
    }
}