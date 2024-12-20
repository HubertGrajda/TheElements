using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonAction : MonoBehaviour
    {
        private Button _button;
        protected virtual bool IsValid => true;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            Prepare();
            
            if (!IsValid) return;
            
            _button.onClick.AddListener(OnClick);
        }
        
        protected abstract void OnClick();

        protected abstract void Prepare();
    }
}