using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Player
{
    public class PlayerEvents : MonoBehaviour
    {
        public UnityAction Step;
        public UnityAction Jump;
    
        public void OnStep() => Step?.Invoke();
        public void OnJump() => Jump?.Invoke();
    }
}