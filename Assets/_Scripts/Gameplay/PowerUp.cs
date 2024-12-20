using _Scripts.Managers;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts
{
    public class PowerUp : MonoBehaviour, ICollectable     
    {
        [SerializeField] private AudioClip collectingSound;
        [SerializeField] private ElementType powerUpType;
        [SerializeField] private int value;

        public void Collect()
        {
            AudioSource.PlayClipAtPoint(collectingSound, transform.position);
        
            if (PlayerManager.Instance.TryGetPlayerComponent(out PlayerExperienceSystem experienceSystem))
            {
                experienceSystem.AddExperience(powerUpType, value);
                Destroy(gameObject);
            }
        }
    }
}