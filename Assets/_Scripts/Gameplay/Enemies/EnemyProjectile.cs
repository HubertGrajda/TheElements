using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected VisualEffect vfx;
    [SerializeField] protected GameObject model;
    [SerializeField] protected int damage;
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    
    private void OnEnable()
    {
        if (vfx != null)
        {
            vfx.Play();
        }
    }
    
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Constants.Tags.PLAYER_TAG) &&
            collision.collider.TryGetComponent(out PlayerHealthSystem playerHealthSystem))
        {
            playerHealthSystem.TakeDamage(damage); 
        }
    }
}
