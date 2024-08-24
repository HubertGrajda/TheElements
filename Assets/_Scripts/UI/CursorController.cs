using _Scripts.Managers;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Vector3 cursorOffset;
    [SerializeField] private GameObject defaultPointer;

    private bool _active;
    private GameObject _currentPointer;
    private void Start()
    {
        Managers.UIManager.onViewOpened += view => ToggleCursor(view,true);
        Managers.UIManager.onViewClosed += view => ToggleCursor(view,false);
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        _currentPointer = defaultPointer;
    }
    
    private void ToggleCursor(View view, bool show)
    {
        if(!view.ShowCursor) return;

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
        if(!_active) return;
        
        var ray = Managers.CamerasManager.CameraUI.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var raycastHit, Mathf.Infinity, ~LayerMask.NameToLayer("UI"))) return;

        _currentPointer.transform.position = raycastHit.point + cursorOffset;
    }
}
