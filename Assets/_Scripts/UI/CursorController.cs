using _Scripts.Managers;
using UnityEngine;

namespace UI
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Vector3 cursorOffset;
        [SerializeField] private GameObject defaultPointer;

        private bool _active;
        private GameObject _currentPointer;

        private UIManager _uiManager;
    
        private void Start()
        {
            _uiManager = UIManager.Instance;
            _currentPointer = defaultPointer;
        
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        
            AddListeners();
        }

        private void AddListeners()
        {
            _uiManager.OnViewOpened += view => ToggleCursor(view, true);;
            _uiManager.OnViewClosed += view => ToggleCursor(view,false);
        }

        private void RemoveListeners()
        {
            if (_uiManager == null) return;
        
            _uiManager.OnViewOpened -= view => ToggleCursor(view, true);;
            _uiManager.OnViewClosed -= view => ToggleCursor(view,false);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void ToggleCursor(View view, bool show)
        {
            if (!view.ShowCursor) return;

            if (_currentPointer == null)
            {
                _currentPointer = defaultPointer;
            }
        
            _currentPointer.SetActive(show);
            _active = show;
        }

        public void ChangePointer(GameObject pointer)
        {
            _currentPointer = pointer;
        }
    
        private void Update()
        {
            if (!_active) return;
        
            var ray = CameraManager.Instance.CameraUI.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, ~LayerMask.NameToLayer("UI"))) return;

            _currentPointer.transform.position = raycastHit.point + cursorOffset;
        }
    }
}