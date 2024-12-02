using _Scripts.Managers;
using UnityEngine;

namespace UI
{
    public class SpellSelectorButton : ButtonAction
    {
        [SerializeField] private ElementType typeToSelect;

        protected override bool IsValid => base.IsValid && _bendingStateMachine != null;

        private PlayerBendingStateMachine _bendingStateMachine;
        
        protected override void Prepare()
        {
            if (PlayerManager.Instance.TryGetPlayerController(out var playerController))
            {
                playerController.TryGetComponent(out _bendingStateMachine);
            }
        }

        protected override void OnClick()
        { 
            if (!IsValid) return;
            
            _bendingStateMachine.ChangeState(typeToSelect);
        }
    }
}