using _Scripts.Managers;

public class PlayerHealthSystem : BaseHealthSystem
{
    public override void Death()
    {
        if(isDead) return;
        
        PlayerManager.Instance.death.Invoke();

        base.Death();
    }
}
