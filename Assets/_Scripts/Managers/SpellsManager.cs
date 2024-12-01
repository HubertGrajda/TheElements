using System;
using _Scripts.Spells;
using UnityEngine;

namespace _Scripts.Managers
{
    [RequireComponent(typeof(SpellLimiterController))]
    public class SpellsManager : Singleton<SpellsManager>
    {
        public SpellLimiterController SpellLimiterController { get; private set; }

        public Action<Spell> OnSelectedSpellChanged;
        public Action<ElementType, int> OnSelectedElementChanged;

        private void Start()
        {
            SpellLimiterController = GetComponent<SpellLimiterController>();
        }
    }
}