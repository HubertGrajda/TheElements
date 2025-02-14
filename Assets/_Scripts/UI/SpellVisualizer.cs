using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class SpellVisualizer : MonoBehaviour
    {
        [SerializeField] private ElementType visualizedType;

        [SerializeField] private Image selectedImage;
        [SerializeField] private Image spellImage;
        [SerializeField] private Image spellImageFill;
        
        private Slider _slider;
        private SpellsManager _spellsManager;
        private SpellLauncher _playerSpellLauncher;
        private SpellLimiter _visualizedSpellLimiter;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _spellsManager = SpellsManager.Instance;
            PlayerManager.Instance.TryGetPlayerComponent(out _playerSpellLauncher);
            
            AddListeners();
        }

        private void OnDestroy() => RemoveListeners();

        private void AddListeners()
        {
            _spellsManager.OnActiveSpellChanged += OnActiveSpellChanged;
            _spellsManager.OnActiveElementChanged += OnSelectedElementChanged;
            _playerSpellLauncher.OnSpellUsed += OnSpellUsed;
        }
        
        private void RemoveListeners()
        {
            _spellsManager.OnActiveSpellChanged -= OnActiveSpellChanged;
            _spellsManager.OnActiveElementChanged -= OnSelectedElementChanged;
            _playerSpellLauncher.OnSpellUsed -= OnSpellUsed;
        }

        private void OnSelectedElementChanged(ElementType elementType)
        {
            if (selectedImage == null) return;
            
            selectedImage.gameObject.SetActive(elementType == visualizedType);
        }

        private void OnActiveSpellChanged(ElementType elementType, SpellConfig spellConfig)
        {
            if (visualizedType != elementType) return;
            
            VisualizeSliderImages(spellConfig);
            VisualizeSliderValue(spellConfig);
        }

        private void OnSpellUsed(SpellConfig spell)
        {
            if (visualizedType != spell.ElementType) return;
            
            VisualizeSliderValue(spell);
        }

        private void VisualizeSliderImages(SpellConfig spell)
        {
            if (spell == null)
            {
                spellImage.gameObject.SetActive(false);
                spellImageFill.gameObject.SetActive(false);
                return;
            }
            
            spellImage.gameObject.SetActive(true);
            spellImageFill.gameObject.SetActive(true);
            
            var spellUIConfig = spell.SpellUIConfig;
            
            if (spellImageFill != null)
            {
                spellImageFill.sprite = spellUIConfig.SpellSprite;
            }

            if (spellImage != null)
            {
                spellImage.sprite = spellUIConfig.SpellSprite;
            }
        }

        private void VisualizeSliderValue(SpellConfig spellConfig)
        {
            if (spellConfig == null) return;
            
            if (_visualizedSpellLimiter != null)
            {
                _visualizedSpellLimiter.OnCurrentValueChanged -= OnCurrentValueChanged;
                _visualizedSpellLimiter = null;
            }
            
            if (_playerSpellLauncher.TryGetLimiter(spellConfig, out var limiter))
            {
                _visualizedSpellLimiter = limiter;
                _slider.maxValue = limiter.MaxValue;
                _slider.minValue = limiter.MinValue;
                _slider.value = limiter.CurrentValue;
                
                limiter.OnCurrentValueChanged += OnCurrentValueChanged;
            }
            else
            {
                _slider.value = _slider.minValue;
            }
        }
        
        private void OnCurrentValueChanged(float currentValue)
        {
            _slider.value = currentValue;
        }
    }
}