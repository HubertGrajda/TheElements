using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(fileName = "new spellsConfig", menuName = "Spells/SpellsConfig")]
    public class SpellsConfig : ScriptableObject
    {
        [SerializeField] private List<SpellConfig> spellsConfigs;
        
        public List<SpellConfig> ConvertToListOfSpellConfigs(List<string> source)
        {
            var result = new List<SpellConfig>();

            foreach (var name in source)
            {
                if (!TryGetSpellByName(name, out var spellConfig)) continue;
                
                result.Add(spellConfig);
            }

            return result;
        }
        
        private bool TryGetSpellByName(string spellName, out SpellConfig spellConfig) => 
            spellConfig = spellsConfigs.FirstOrDefault(x => x.name == spellName);
    }
}