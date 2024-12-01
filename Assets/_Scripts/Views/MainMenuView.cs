using UnityEngine;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private GameObject pointer;
    
        protected override void Start()
        {
            base.Start();
        
            UIManager.CursorController.ChangePointer(pointer);
            Show();
        }
    }
}