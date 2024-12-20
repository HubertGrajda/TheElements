using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Spells
{
    public class SmallRocks : Spell
    {
        [SerializeField] private List<SimpleProjectile> projectiles;
        [SerializeField] private float shootingDelay;
        
        private float _timer;
        private int _currentProjectileIndex;

        public override void Cast()
        {
            base.Cast();
            
            foreach (var projectile in projectiles)
            {
                projectile.Prepare(SpellLauncher.SpawnPoint);
            }
        }

        public override void Launch()
        {
            base.Launch();

            StartCoroutine(StartShooting());
        }

        private IEnumerator StartShooting()
        {
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