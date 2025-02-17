using _Scripts.Inputs;
using UnityEngine;

namespace _Scripts.UI
{
    public class PauseView : InputView
    {
        [SerializeField] private Menu defaultMenu;

        protected override void AssignInputAction()
        {
            inputAction = InputsManager.Instance.UIActions.PauseView;
        }

        protected override void Show()
        {
            if (!CanBeShown) return;
            
            defaultMenu.Open();
        
            base.Show();
        }
    }
}