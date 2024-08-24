using UnityEngine;

public class PowerUp : MonoBehaviour//, IInteractable       
{
    [SerializeField] private AudioClip collectingSound;
    [SerializeField] private ElementType powerUpType;
    [SerializeField] private int value;

    public void CollectPowerUp(GameObject player)
    {
        var experienceSystem = player.GetComponent<PlayerExperienceSystem>();
        AudioSource.PlayClipAtPoint(collectingSound, transform.position);
        experienceSystem.AddExperience(value, powerUpType);
        Destroy(gameObject);
    }

}
