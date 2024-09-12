using _Scripts.Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.Instance.SetUpPlayerRef(gameObject);
    }
}
