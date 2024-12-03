using UnityEngine;

public class AIHealthSystem : BaseHealthSystem
{
    private Animator _anim;
    
    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
    }
    
    public override void TakeDamage(int damage)
    {
        _anim.SetTrigger(Constants.AnimationNames.DAMAGED);
        
        base.TakeDamage(damage);
    }
    
    public override void Death()
    {
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        
        base.Death();
    }
}
