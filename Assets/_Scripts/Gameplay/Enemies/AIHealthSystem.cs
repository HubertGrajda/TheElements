using UnityEngine;

public class AIHealthSystem : BaseHealthSystem
{
    private Animator _anim;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    
    public override void TakeDamage(int damage)
    {
        _anim.SetTrigger(Constants.AnimationNames.DAMAGED);
        
        base.TakeDamage(damage);
    }
    
    public override void Death()
    {
        GetComponent<AIStateMachine>().enabled = false;
        _anim.SetTrigger(Constants.AnimationNames.DEATH);
        
        base.Death();
    }
}
