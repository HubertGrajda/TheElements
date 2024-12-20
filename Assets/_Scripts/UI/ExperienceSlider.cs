using System.Collections;
using _Scripts.Managers;
using _Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class ExperienceSlider : MonoBehaviour
    {
        [SerializeField] private ElementType elementType;

        [SerializeField] private float valueChangingDuration = 0.7f;

        private PlayerExperienceSystem _experienceSystem;
        private Coroutine _valueChangingCoroutine;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            if (PlayerManager.Instance.TryGetPlayerComponent(out _experienceSystem))
            {
                AddListeners();
            }
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            _experienceSystem.OnExperienceChanged += OnExperienceChanged;
        }

        private void RemoveListeners()
        {
            if (_experienceSystem == null) return;
            
            _experienceSystem.OnExperienceChanged -= OnExperienceChanged;
        }
        
        private void OnExperienceChanged(ElementType type, float experience)
        {
            if (type != elementType) return;

            if (_valueChangingCoroutine != null)
            {
                StopCoroutine(_valueChangingCoroutine);
            }

            _valueChangingCoroutine = StartCoroutine(ChangeValueOverTime(valueChangingDuration, experience));
        }

        private IEnumerator ChangeValueOverTime(float time, float value)
        {
            var timer = 0f;
            var initialValue = _slider.value;
            
            while (timer < time)
            {
                _slider.value = Mathf.Lerp(initialValue, value, timer / time);
                timer += Time.deltaTime;
                yield return null;
            }

            _slider.value = value;
        }
    }
}