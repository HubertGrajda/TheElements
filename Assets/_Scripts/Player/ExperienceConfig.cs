using System.Linq;
using UnityEngine;

namespace _Scripts.Player
{
    [CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Experience Config")]
    public class ExperienceConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<int, float> levelToExperienceThresholds;

        public bool TryGetLevelForExperience(float experience, out int level)
        {
            level = levelToExperienceThresholds.FirstOrDefault(x => x.Value > experience).Key;
            
            return level != default;
        }

        public bool TryGetRequiredExperienceForLevel(int level, out float requiredExperience) =>
            levelToExperienceThresholds.TryGetValue(level, out requiredExperience);
    }
}