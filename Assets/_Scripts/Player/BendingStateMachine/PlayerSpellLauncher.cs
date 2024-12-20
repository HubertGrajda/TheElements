using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerSpellLauncher : SpellLauncher
    {
        private PlayerBendingStateMachine _playerBendingStateMachine;
        private PlayerInputs.PlayerActions _playerActions;
        private SpellsManager _spellsManager;
    
        private void Start()
        {
            _playerBendingStateMachine = GetComponent<PlayerBendingStateMachine>();
            _playerActions = InputsManager.Instance.PlayerActions;
            _spellsManager = SpellsManager.Instance;
        
            AddListeners();
        }
    
        private void OnDestroy() => RemoveListeners();

        private void AddListeners()
        {
            _playerActions.CastSpell.started += OnCastSpellInputStarted;
            _playerActions.CastSpell.canceled += OnCastSpellInputCanceled;
            _spellsManager.OnSelectedSpellChanged += OnSelectedSpellChanged;
        }
    
        private void RemoveListeners()
        {
            if (_playerBendingStateMachine == null) return;
        
            _playerActions.CastSpell.started -= OnCastSpellInputStarted;
            _playerActions.CastSpell.canceled -= OnCastSpellInputCanceled;
            _spellsManager.OnSelectedSpellChanged -= OnSelectedSpellChanged;
        }

        private void OnCastSpellInputStarted(InputAction.CallbackContext ctx) => UseSpell();

        private void OnCastSpellInputCanceled(InputAction.CallbackContext ctx)
        {
            CurrentCastingBehaviour.ToggleCastingAnimation(this, false);
            CancelSpell();
        }

        private void OnSelectedSpellChanged(SpellConfig obj)
        {
            var castingBehaviour = CurrentCastingBehaviour;
            if (castingBehaviour == null) return;
        
            castingBehaviour.ToggleCastingAnimation(this, false);
            CancelSpell();
        }
    
        protected override bool TryGetSpellToUse(out SpellConfig spellConfig)
        {
            spellConfig = _playerBendingStateMachine.CurrentBendingState?.SelectedSpell;
            return spellConfig != null;
        }

        public override Vector3 GetTarget()
        {
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);

            const float maxDistance = 50f;
        
            return Physics.Raycast(ray, out var hit, maxDistance) 
                ? hit.point 
                : ray.GetPoint(maxDistance);
        }
    }
}