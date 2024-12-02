using _Scripts.Managers;
using _Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace UI
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
        private SpellLimiter _visualizedSpellLimiter;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _spellsManager = SpellsManager.Instance;
            AddListeners();
        }

        private void OnDestroy() => RemoveListeners();

        private void AddListeners()
        {
            _spellsManager.OnSelectedSpellChanged += OnSelectedSpellChanged;
            _spellsManager.OnSelectedElementChanged += OnSelectedElementChanged;
        }
        
        private void RemoveListeners()
        {
            if (_spellsManager == null) return;
            
            _spellsManager.OnSelectedSpellChanged -= OnSelectedSpellChanged;
            _spellsManager.OnSelectedElementChanged -= OnSelectedElementChanged;
        }

        private void OnSelectedElementChanged(ElementType elementType, int index)
        {
            if (selectedImage == null) return;
            
            selectedImage.gameObject.SetActive(elementType == visualizedType);
        }

        private void OnSelectedSpellChanged(Spell spell)
        {
            if (visualizedType != spell.SpellData.ElementType) return;

            VisualizeSpell(spell);
        }

        private void VisualizeSpell(Spell spell)
        {
            VisualizeSliderImages(spell);
            VisualizeSliderValue(spell);
        }

        private void VisualizeSliderImages(Spell spell)
        {
            var spellUIConfig = spell.SpellData.SpellUIConfig;
            
            if (spellImageFill != null)
            {
                spellImageFill.sprite = spellUIConfig.SpellSprite;
            }

            if (spellImage != null)
            {
                spellImage.sprite = spellUIConfig.SpellSprite;
            }
        }

        private void VisualizeSliderValue(Spell spell)
        {
            if (_visualizedSpellLimiter != null)
            {
                _visualizedSpellLimiter.OnCurrentValueChanged -= OnCurrentValueChanged;
                _visualizedSpellLimiter = null;
            }
            
            if (spell.TryGetSpellLimiter(out var limiter))
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