using _Scripts.Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerManager.Instance.SetUpPlayerRef(this);
    }
}
