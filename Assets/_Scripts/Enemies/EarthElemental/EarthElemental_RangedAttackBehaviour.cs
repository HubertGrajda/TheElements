using _Scripts.Managers;
using UnityEngine;

public class EarthElemental_RangedAttackState : BaseRangeAttackBehaviour
{
    public override void OnLaunchRangeAttack()
    {
        AddForceToProjectiles();
    }

    public override void OnCastRangeAttack()
    {
    }

    private void AddForceToProjectiles()
    {
        var projectile = ObjectPoolingManager.Instance.SpawnFromPool(stats.projectile, projectileSpawnPoint.position, transform.localRotation);
        var direction = (target.position - transform.position).normalized;

        projectile.Rigidbody.AddForce(direction * stats.shootingForce, ForceMode.Impulse);
    }
}
