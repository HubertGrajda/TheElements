using _Scripts.Spells;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class SpellSelectionButton : MonoBehaviour
    {
        [SerializeField] private SpellConfig spell;

        [SerializeField] private Image spellImage;
        [SerializeField] private Image lockedImage;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color deselectedColor;
        
        [SerializeField] private Vector3 lockedAnimationPunch = new (0.2f, 0.2f, 0.2f);
        [SerializeField] private float lockedAnimationDuration = 0.2f;
        
        private Button _button;
        private SpellsManager _spellsManager;
        
        private State CurrentState
        {
            get
            {
                if (_spellsManager.UnlockedSpells.Contains(spell))
                {
                    return _spellsManager.IsSpellSelected(spell)
                        ? State.Selected
                        : State.Deselected;
                }
                
                return State.Locked;
            }
        }

        private enum State
        {
            Locked = 0,
            Deselected = 1,
            Selected = 2
        }
        
        private void Awake()
        {
            if (!spell) return;
            
            _button = GetComponent<Button>();
            _spellsManager = SpellsManager.Instance;
            
            SetUpByState();
            SetUpImage();
            AddListeners();
        }

        private void AddListeners()
        {
            _button.onClick.AddListener(OnClick);
            _spellsManager.OnSpellUnlocked += OnSpellUnlocked;
            _spellsManager.OnSpellSelected += OnSpellSelected;
            _spellsManager.OnSpellDeselected += OnSpellDeselected;
        }

        private void RemoveListeners()
        {
            if (_button)
            {
                _button.onClick.RemoveListener(OnClick);
            }

            if (_spellsManager)
            {
                _spellsManager.OnSpellUnlocked -= OnSpellUnlocked;
                _spellsManager.OnSpellSelected -= OnSpellSelected;
                _spellsManager.OnSpellDeselected -= OnSpellDeselected;
            }
        }

        private void OnDestroy() => RemoveListeners();

        private void SetUpByState()
        {
            switch (CurrentState)
            {
                case State.Locked:
                {
                    lockedImage.gameObject.SetActive(true);
                    break;
                }
                case State.Deselected:
                {
                    OnSpellDeselected(spell);
                    break;
                }
                case State.Selected:
                {
                    OnSpellSelected(spell);
                    break;
                }
            }
        }
        
        private void SetUpImage()
        {
            if (!spellImage || !spell) return;

            spellImage.sprite = spell.SpellUIConfig.SpellSprite;
        }

        private void OnClick()
        {
            switch (CurrentState)
            {
                case State.Locked:
                {
                    if (!_spellsManager.TryToUnlockSpell(spell))
                    {
                        LockedPunchAnimation();
                    }
                    break;
                }
                case State.Deselected:
                {
                    _spellsManager.SelectSpell(spell);
                    break;
                }
                case State.Selected:
                {
                    _spellsManager.DeselectSpell(spell);
                    break;
                }
            }
        }

        private void ChangeButtonColor(Color color)
        {
            var colorBlock = _button.colors;
            colorBlock.normalColor = color;
            colorBlock.selectedColor = color;
            _button.colors = colorBlock;
        }
        
        private void OnSpellUnlocked(SpellConfig spellConfig)
        {
            if (spellConfig != spell) return;
            
            lockedImage.gameObject.SetActive(false);
        }
        
        private void OnSpellDeselected(SpellConfig spellConfig)
        {
            if (spellConfig != spell) return;
            
            ChangeButtonColor(deselectedColor);
        }

        private void OnSpellSelected(SpellConfig spellConfig)
        {
            if (spellConfig != spell) return;
            
            ChangeButtonColor(selectedColor);
        }

        private void LockedPunchAnimation()
        {
            if (!lockedImage) return;
            
            lockedImage.transform
                .DOPunchScale(lockedAnimationPunch, lockedAnimationDuration)
                .OnComplete(() => lockedImage.transform.localScale = Vector3.one)
                .SetUpdate(true);
        }
    }
}