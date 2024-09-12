using _Scripts.Managers;
using UnityEngine;

public class FireElemental_RangedAttackBehaviour : BaseRangeAttackBehaviour
{
    
    private EnemyProjectile _projectile;
    private Rigidbody _rb;
    private readonly Vector3 _offset = new Vector3(0, 0.5f, 0); // temp solution to shoot higher

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
        _projectile = ObjectPoolingManager.Instance.SpawnFromPool(stats.projectile, projectileSpawnPoint.position, transform.rotation);
        _projectile.transform.SetParent(projectileSpawnPoint);
        _rb = _projectile.Rigidbody;
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }
    
    private void FireProjectile()
    {
        _projectile.transform.SetParent(null);
        var direction = (target.position -  _projectile.transform.position + _offset).normalized;
        
        _rb = _projectile.Rigidbody;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddForce(direction * stats.shootingForce, ForceMode.Impulse);
    }

}
