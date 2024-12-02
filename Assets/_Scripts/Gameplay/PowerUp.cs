using _Scripts.Managers;
using UnityEngine;

public class PowerUp : MonoBehaviour, ICollectable     
{
    [SerializeField] private AudioClip collectingSound;
    [SerializeField] private ElementType powerUpType;
    [SerializeField] private int value;

    public void Collect()
    {
        AudioSource.PlayClipAtPoint(collectingSound, transform.position);
        
        var experienceSystem = PlayerManager.Instance.ExperienceSystem;
        experienceSystem.AddExperience(powerUpType, value);
        Destroy(gameObject);
    }
}
