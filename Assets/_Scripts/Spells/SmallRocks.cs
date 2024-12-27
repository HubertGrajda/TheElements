using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public class SmallRocks : Spell
    {
        [SerializeField] private SimpleProjectile projectilePrefab;
        [SerializeField] private float shootingDelay;
        [SerializeField] private int projectilesCount;
        
        private readonly List<SimpleProjectile> _preparedProjectiles = new();

        public override void Cast()
        {
            base.Cast();
            for (var i = 0; i < projectilesCount; i++)
            {
                var projectile = ObjectPoolingManager.Instance.GetFromPool(projectilePrefab);
                projectile.Prepare(SpellLauncher);
                _preparedProjectiles.Add(projectile);
            }
        }

        public override void Launch()
        {
            base.Launch();

            StartCoroutine(StartShooting());
        }

        private IEnumerator StartShooting()
        {
            var projectiles = new List<SimpleProjectile>(_preparedProjectiles);
            _preparedProjectiles.Clear();
            
            foreach (var projectile in projectiles)
            {
                if (projectile == null) continue;
                
                projectile.Shoot( (SpellLauncher.GetTarget() - transform.position).normalized * speed);
                yield return new WaitForSeconds(shootingDelay);
            }
            
            Disable();
        }
    }
}