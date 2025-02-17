using System.Collections.Generic;
using System.Linq;
using _Scripts.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.UI
{
    public class SpellsView : InputView
    {
        [SerializeField] private List<SpellsMenu> menus;
        
        protected override void AssignInputAction()
        {
            inputAction = InputsManager.Instance.UIActions.SpellsView;
        }

        protected override void Show()
        {
            if (!CanBeShown) return;
        
            base.Show();
        }

        public override void Hide()
        {
            if (!IsShown) return;
            
            base.Hide();
        }
        
        private void OnNumKeyStarted(InputAction.CallbackContext context)
        {
            var value = (int)context.ReadValue<float>();
            
            OpenMenuByIndex(value);
        }

        private void OpenMenuByIndex(int index)
        {
            var menu = menus.FirstOrDefault(x => x.ElementType.Index == index);
            if (menu == null) return;
            
            menu.Open();
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            InputsManager.Instance.UIActions.NumKeys.started += OnNumKeyStarted;
        }
        
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            InputsManager.Instance.UIActions.NumKeys.started -= OnNumKeyStarted;
        }
    }
}