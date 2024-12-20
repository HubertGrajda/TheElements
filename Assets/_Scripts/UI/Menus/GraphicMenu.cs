using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class GraphicMenu : MonoBehaviour
    {
        public TMP_Dropdown resolutionDropdown;
    
        private Resolution[] _resolutions;
    
        private void Start()
        { 
            _resolutions = Screen.resolutions;
        
            SetResolutionDropdown();
        }

        private void SetResolutionDropdown()
        {
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