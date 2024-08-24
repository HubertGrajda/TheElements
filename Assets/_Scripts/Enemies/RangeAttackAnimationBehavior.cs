using UnityEngine;

public class RangeAttackAnimationBehavior : StateMachineBehaviour
{
    private AIState_RangedAttack rangeAttackState;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rangeAttackState = animator.gameObject.GetComponent<AIStateMachine>().RangedAttackState;
        rangeAttackState.BlockStateSwitching(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rangeAttackState.BlockStateSwitching(false);
    }
}
