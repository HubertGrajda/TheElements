using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class ElementTypeSelectionView : InputView
    {
        [SerializeField] private float transitionTime = 0.5f;
        [SerializeField] private float slowedTimeScale = 0.1f;
    
        private CameraManager _cameraManager;

        private const float DEFAULT_TIMESCALE = 1f;
    
        protected override void Start()
        {
            base.Start();
        
            _cameraManager = CameraManager.Instance;
        }

        protected override void AssignInputAction()
        {
            inputAction = InputManager.Instance.Inputs.UIActions.ElementTypeSelectionView;
        }
    
        protected override void ToggleByInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Show();
            }

            if (context.canceled)
            {
                StartCoroutine(GameManager.ChangeTimeScale(DEFAULT_TIMESCALE, transitionTime));
                Hide();
            }
        }

        protected override void Show()
        {
            base.Show();
            _cameraManager.ToggleMainCameraMovement(false);
            StartCoroutine(GameManager.ChangeTimeScale(slowedTimeScale, transitionTime));
        }

        public override void Hide()
        {
            base.Hide();
            _cameraManager.ToggleMainCameraMovement(true);
            StartCoroutine(GameManager.ChangeTimeScale(DEFAULT_TIMESCALE, transitionTime));
        }
    }
}
