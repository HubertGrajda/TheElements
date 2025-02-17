using System.Collections;
using _Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class ExperienceSlider : MonoBehaviour
    {
        [SerializeField] private ElementType elementType;
        [SerializeField] private QuickTextVisualizer experiencePointsPrefab;

        [SerializeField] private TMP_Text currentExperienceText;
        [SerializeField] private TMP_Text requiredExperienceText;
        
        [SerializeField] private float valueChangingDuration = 0.7f;

        private PlayerExperienceSystem _experienceSystem;
        private Coroutine _valueChangingCoroutine;
        private Slider _slider;

        private void Awake()
        {
            if (!PlayerManager.Instance.TryGetPlayerComponent(out _experienceSystem)) return;
            
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            Prepare();
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _experienceSystem.OnExperienceChanged += OnExperienceChanged;
            _experienceSystem.OnLevelChanged += OnLevelChanged;
        }

        private void OnLevelChanged(ElementType element, int previousLevel, int currentLevel)
        {
            if (element != elementType) return;
            
            SetMinMaxValues(
                _experienceSystem.GetRequiredExperience(previousLevel),
                _experienceSystem.GetRequiredExperience(currentLevel));
        }

        private void RemoveListeners()
        {
            if (_experienceSystem == null) return;
            
            _experienceSystem.OnExperienceChanged -= OnExperienceChanged;
        }

        private void Prepare()
        {
            var currentLevel = _experienceSystem.GetCurrentLevel(elementType);
            
            OnLevelChanged(elementType, currentLevel -1, currentLevel);
            SetValue(_experienceSystem.GetCurrentExperience(elementType));
        }
        
        private void OnExperienceChanged(ElementType type, float experience, float difference)
        {
            if (type != elementType) return;

            if (_valueChangingCoroutine != null)
            {
                StopCoroutine(_valueChangingCoroutine);
            }
            
            _valueChangingCoroutine = StartCoroutine(ChangeValueOverTime(valueChangingDuration, experience));
            experiencePointsPrefab.Show($"{(int)difference}");
        }

        private IEnumerator ChangeValueOverTime(float time, float value)
        {
            var timer = 0f;
            var initialValue = _slider.value;
            
            while (timer < time)
            {
                SetValue(Mathf.Lerp(initialValue, value, timer / time));
                timer += Time.deltaTime;
                yield return null;
            }

            SetValue(value);
        }

        private void SetValue(float value)
        {
            _slider.value = value;

            if (currentExperienceText)
            {
                currentExperienceText.text = $"{(int)value}";
            }
        }

        private void SetMinMaxValues(float min, float max)
        {
            _slider.minValue = min;
            _slider.maxValue = max;

            if (requiredExperienceText)
            {
                requiredExperienceText.text = $"{(int)_slider.maxValue}";
            }
        }
    }
}