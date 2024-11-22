using System.Collections;
using UnityEngine;

public class FireballProjectile : EnemyProjectile
{
    [SerializeField] private float vfxOnHitSize = 3f;
    [SerializeField] private float vfxOnHitGrowingTime = 0.2f;
    [SerializeField] private float vfxStartSize = 1f;
    [SerializeField] private float timeToDestroy = 3f;
    
    private const string VFX_SIZE_PROPERTY = "Size";
    private bool _exploded;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (_exploded) return;
        
        StartCoroutine(HandleCollision());
    }
    
    private void OnCollision()
    {
        gameObject.SetActive(false);
        model.SetActive(true);
        vfx.SetFloat(VFX_SIZE_PROPERTY, vfxStartSize);
        _exploded = false;
    }

    private IEnumerator HandleCollision()
    {
        model.SetActive(false);
        _exploded = false;
        yield return ChangeSizeOverTime(vfxOnHitSize, vfxOnHitGrowingTime);
        yield return ChangeSizeOverTime(0f, timeToDestroy);
        OnCollision();
    }
    
    private IEnumerator ChangeSizeOverTime(float finalSize, float resizingTime)
    {
        var timer = 0f;
        var initialSize = vfx.GetFloat(VFX_SIZE_PROPERTY);
            
        while (timer < resizingTime)
        {
            var value = Mathf.Lerp(initialSize, finalSize, timer / resizingTime);
            
            vfx.SetFloat(VFX_SIZE_PROPERTY, value);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
