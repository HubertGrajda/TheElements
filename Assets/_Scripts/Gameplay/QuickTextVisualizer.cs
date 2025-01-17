using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class QuickTextVisualizer : MonoBehaviour, IPoolable
    {
        [SerializeField] private TMP_Text text;
        
        [SerializeField] private float punchForce = 0.3f;
        [SerializeField] private float punchDuration = 0.5f;
        [SerializeField] private float fadeInDuration = 0.2f;
        [SerializeField] private float fadeOutDuration = 1f;
        
        public void Show(string value)
        {
            text.DOKill();
            text.alpha = 0;
            text.text = value;
            gameObject.SetActive(true);
            text.DOFade(1, fadeInDuration).OnComplete(Hide);
            text.transform.DOPunchPosition(transform.up * punchForce, punchDuration, 1, 0);
        }

        private void Hide()
        {
            text.DOFade(0, fadeOutDuration).OnComplete(() =>
            {
                gameObject.SetActive(false);
                text.DOKill(true);
            });
        }
    }
}