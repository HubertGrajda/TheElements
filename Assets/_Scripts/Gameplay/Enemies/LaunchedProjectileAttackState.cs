using _Scripts.Managers;
using UnityEngine;

public class LaunchedProjectileAttackState : BaseRangeAttackBehaviour
{
    public override void OnLaunchRangeAttack()
    {
        AddForceToProjectiles();
    }

    private void AddForceToProjectiles()
    {
        var projectile = ObjectPoolingManager.Instance.SpawnFromPool(Stats.Projectile, projectileSpawnPoint.position, transform.localRotation);
        var direction = (Target.position - transform.position).normalized;

        projectile.Rigidbody.AddForce(direction * Stats.ShootingForce, ForceMode.Impulse);
    }
}
