using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GraphicsMenu : Menu
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;

        [SerializeField] private Button backBtn;
    
        private Resolution[] _resolutions;
    
        protected override void InitUIElements()
        {
            backBtn.onClick.AddListener(OnBackButtonClicked);
        
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            qualityDropdown.onValueChanged.AddListener(SetQuality);
        }
    
        protected override void Start()
        {
            base.Start();
        
            _resolutions = Screen.resolutions;
            var options = new List<string>();
            var currentResolutionOption = 0;

            resolutionDropdown.ClearOptions();

            for (var i = 0; i < _resolutions.Length; i++)
            {
                var option = _resolutions[i].width + "x" + _resolutions[i].height;

                options.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                {
                    currentResolutionOption = i;
                }
            }
        
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionOption;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetQuality(int optionIndex)
        {
            QualitySettings.SetQualityLevel(optionIndex);
        }

        public void SetResolution(int optionIndex)
        {
            var res = _resolutions[optionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }

        public void PlayInWindow(bool windowed)
        {
            Screen.fullScreen = !windowed;
        }
    }
}