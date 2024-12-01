using _Scripts.Managers;
using UnityEngine;

namespace UI
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
            defaultMenu.Open();
        
            base.Show();
        }
    }
}