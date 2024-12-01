using _Scripts.Managers;
using UnityEngine;

public class CastedProjectileAttackBehaviour : BaseRangeAttackBehaviour
{
    private EnemyProjectile _projectile;
    private Rigidbody _rb;
    private readonly Vector3 _offset = new(0, 0.5f, 0);

    public override void OnCastRangeAttack()
    {
        CreateProjectile();
    }
    public override void OnLaunchRangeAttack()
    {
        FireProjectile();
    }

    private void CreateProjectile()
    {
        _projectile = ObjectPoolingManager.Instance.GetFromPool(Stats.Projectile);
        _projectile.gameObject.SetActive(true);
        _projectile.transform.position = projectileSpawnPoint.position;
        _projectile.transform.SetParent(projectileSpawnPoint);
        
        _rb = _projectile.Rigidbody;
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }
    
    private void FireProjectile()
    {
        _projectile.transform.SetParent(null);
        var direction = (Target.position - _projectile.transform.position + _offset).normalized;
        
        _rb = _projectile.Rigidbody;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddForce(direction * Stats.ShootingForce, ForceMode.Impulse);
    }

}
