using System.Collections;
using UnityEngine;

public class FireballProjectile : EnemyProjectile
{
    private bool _handled;
    private const string VFX_SIZE_PROPERTY = "Size";
    
    [SerializeField] private float vfxOnHitSize = 3f;
    [SerializeField] private float vfxOnHitGrowingTime = 0.2f;
    [SerializeField] private float vfxStartSize = 1f;
    [SerializeField] private float timeToDestroy = 3f;
    
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (_handled) return;
        
        StartCoroutine(HandleCollision());
    }
    
    public override void OnSpawnFromPool()
    {
    }

    public override void ReturnToPool()
    {
        gameObject.SetActive(false);
        model.SetActive(true);
        vfx.SetFloat(VFX_SIZE_PROPERTY, vfxStartSize);
        _handled = false;
    }

    private IEnumerator HandleCollision()
    {
        model.SetActive(false);
        _handled = false;
        yield return StartCoroutine(Grow(vfxOnHitSize, vfxOnHitGrowingTime));
        yield return StartCoroutine(Deactivate(timeToDestroy));
        ReturnToPool();
    }
    
    //TODO:
    private IEnumerator Grow(float finalSize, float resizingTime)
    {
        var timer = 0f;

        while (timer < resizingTime)
        {
            var value = finalSize * (timer / resizingTime);
            
            vfx.SetFloat(VFX_SIZE_PROPERTY, value);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator Deactivate(float resizingTime)
    {
        var timer = 0f;

        var currSize = vfx.GetFloat(VFX_SIZE_PROPERTY);
        while (timer < resizingTime)
        {
            var value = currSize - currSize * (timer / resizingTime);
            vfx.SetFloat(VFX_SIZE_PROPERTY, value);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
