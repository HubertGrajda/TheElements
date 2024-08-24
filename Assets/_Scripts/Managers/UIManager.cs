using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private CursorController cursorController;
        public CursorController CursorController => cursorController;
    
        private readonly Stack<Menu> _previousMenus = new();
        public Menu CurrentMenu { get; private set; }
        public View CurrentView { get; set; }

        public UnityAction<View> onViewOpened;
        public UnityAction<View> onViewClosed;

        private void Start()
        {
            Managers.InputManager.Inputs.UIActions.Enable();
        }

        public void SetCurrentMenu(Menu newCurrentMenu)
        {
            var previousMenu = CurrentMenu == null ? null : CurrentMenu;

            if(previousMenu != null)
            {
                if (!_previousMenus.Contains(newCurrentMenu))
                {
                    _previousMenus.Push(previousMenu);
                }
                else
                {
                    _previousMenus.Pop();
                }
            }
            
            CurrentMenu = newCurrentMenu;
        }

        public void OpenPreviousMenu()
        {
            var previousMenu = _previousMenus.Peek();
        
            if(previousMenu == null) return;
        
            if (CurrentMenu != null)
            {
                CurrentMenu.Close();
            }
        
            previousMenu.Open();
        }
    
        public void HideCurrentView()
        {
            if(CurrentView == null) return;
        
            CurrentView.Hide();
        }
    }
}