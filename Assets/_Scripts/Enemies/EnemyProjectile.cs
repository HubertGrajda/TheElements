using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected VisualEffect vfx;
    [SerializeField] protected GameObject model;
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
        if (collision.collider.CompareTag(Constants.Tags.PLAYER_TAG))
        {
            collision.collider.GetComponent<PlayerHealthSystem>().Damaged(10); // TODO:
        }
    }

    public virtual void OnSpawnFromPool()
    {
    }

    public virtual void ReturnToPool()
    {
    }
}
