using _Scripts.Managers;

public class PlayerHealthSystem : BaseHealthSystem
{
    public override void Death()
    {
        if (IsDead) return;
        
        PlayerManager.Instance.death.Invoke();

        base.Death();
    }
}
