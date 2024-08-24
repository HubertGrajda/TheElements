using _Scripts.Managers;
using UnityEngine;

public class PlayerHealthSystem : BaseHealthSystem
{
    public override void Death()
    {
        if(isDead) return;
        
        Managers.PlayerManager.death.Invoke();

        base.Death();
    }
}
