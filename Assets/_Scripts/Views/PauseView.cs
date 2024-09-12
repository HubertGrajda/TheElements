using _Scripts.Managers;
using UnityEngine;

namespace UI
{
    public class PauseView : InputView
    {
        [SerializeField] private Menu defaultMenu;

        protected override void Start()
        {
            base.Start();
        
            GameManager.ChangeState(GameManager.GameState.DuringGameplay); // TODO
        }

        protected override void AssignInputAction()
        {
            inputAction = InputManager.Instance.UIActions.PauseView;
        }

        protected override void Show()
        {
            defaultMenu.Open();
        
            base.Show();
        }
    }
}