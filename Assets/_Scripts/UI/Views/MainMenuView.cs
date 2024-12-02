using UnityEngine;

namespace UI
{
    public class MainMenuView : View
    {
        [SerializeField] private GameObject pointer;
    
        protected void Start()
        {
            UIManager.CursorController.ChangePointer(pointer);
            Show();
        }
    }
}