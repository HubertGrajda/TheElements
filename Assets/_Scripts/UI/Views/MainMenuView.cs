using UnityEngine;

namespace _Scripts.UI
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