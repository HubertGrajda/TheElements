using _Scripts.Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        Managers.PlayerManager.SetUpPlayerRef(gameObject);
    }
}
