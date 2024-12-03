using System.Collections.Generic;
using System.Linq;
using UI;
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

        public HUD CurrentHUD { get; private set; }
        
        public UnityAction<View> OnViewOpened;
        public UnityAction<View> OnViewClosed;
        
        private void Start()
        {
            InputsManager.Instance.UIActions.Enable();
        }

        public void AttachHUD(HUD newHUD) => CurrentHUD = newHUD;

        public void SetCurrentMenu(Menu newCurrentMenu)
        {
            var previousMenu = CurrentMenu == null ? null : CurrentMenu;

            if (previousMenu != null)
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
            var filteredMenus = new Stack<Menu>(_previousMenus.Where(x => x != null));
            if (!filteredMenus.TryPeek(out var previousMenu)) return;
        
            if (CurrentMenu != null)
            {
                CurrentMenu.Close();
            }
        
            previousMenu.Open();
        }
    
        public void HideCurrentView()
        {
            if (CurrentView == null) return;
        
            CurrentView.Hide();
        }
    }
}