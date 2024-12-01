using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    public UnityAction step;
    public UnityAction jump;
    public UnityAction<JumpingState.JumpingSubState> jumpingSubStateChanged;
    
    public void OnStep() => step?.Invoke();
    public void OnJump() => jump?.Invoke();
    public void OnJumpingSubStateChanged(JumpingState.JumpingSubState subState) => jumpingSubStateChanged?.Invoke(subState);
    


}
