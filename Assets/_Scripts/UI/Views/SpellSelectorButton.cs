using _Scripts.Player;
using UnityEngine;

namespace _Scripts.UI
{
    public class SpellSelectorButton : ButtonAction
    {
        [SerializeField] private ElementType typeToSelect;

        protected override bool IsValid => base.IsValid && _bendingStateMachine != null;

        private PlayerBendingStateMachine _bendingStateMachine;
        
        protected override void Prepare()
        {
            PlayerManager.Instance.TryGetPlayerComponent(out _bendingStateMachine);
        }

        protected override void OnClick()
        { 
            if (!IsValid) return;
            
            _bendingStateMachine.ChangeState(typeToSelect);
        }
    }
}