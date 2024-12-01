using UnityEngine;

public class RangeAttackAnimationBehavior : StateMachineBehaviour
{
    private RangedAttackAIState _rangeAttackState;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rangeAttackState = animator.gameObject.GetComponent<AIStateMachine>().RangedAttackState;
        _rangeAttackState.BlockStateSwitching(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rangeAttackState.BlockStateSwitching(false);
    }
}
