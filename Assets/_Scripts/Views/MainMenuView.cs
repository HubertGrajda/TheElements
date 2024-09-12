using _Scripts.Managers;
using UnityEngine;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private GameObject pointer;
    
        protected override void Start()
        {
            base.Start();
        
            GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
            UIManager.CursorController.ChangePointer(pointer);
            Show();
        }
    }
}