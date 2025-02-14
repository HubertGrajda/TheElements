using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonAction : MonoBehaviour
    {
        protected Button Button { get; private set; }
        
        protected virtual bool IsValid => true;
        
        private void Awake()
        {
            Button = GetComponent<Button>();
            Prepare();
            
            if (!IsValid) return;
            
            Button.onClick.AddListener(OnClick);
        }
        
        protected abstract void OnClick();

        protected abstract void Prepare();
    }
}