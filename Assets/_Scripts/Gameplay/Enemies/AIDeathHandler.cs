using System.Linq;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.AI
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(AIStateMachine))]
    public class AIDeathHandler : MonoBehaviour
    {
        private HealthSystem _healthSystem;
        private PlayerManager _playerManager;
        private AIStatsConfig _aiStatsConfig;
        
        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _aiStatsConfig = GetComponent<AIStateMachine>().Stats;
            
            _playerManager = PlayerManager.Instance;
            _healthSystem.OnDeath += OnDeath;
        }

        private void OnDeath()
        {
            AddExperience();
        }

        private void AddExperience()
        {
            if (!_playerManager.TryGetPlayerComponent(out PlayerExperienceSystem playerExperienceSystem)) return;
            
            var totalExperience = _aiStatsConfig.Experience;
            var totalDamage = _healthSystem.ElementTypeToDamageTaken.Sum(x => x.Value);

            foreach (var elementToDamage in _healthSystem.ElementTypeToDamageTaken)
            {
                var elementPercentage = (float)elementToDamage.Value / totalDamage;
                var elementExperienceToAdd = totalExperience * elementPercentage;
                
                playerExperienceSystem.AddExperience(elementToDamage.Key, elementExperienceToAdd);
            }
        }
    }
}