using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Audio
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private string mixerVolumeTag;
    
        private Slider _slider;
    
        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.value = AudioManager.Instance.GetCurrentVolume(mixerVolumeTag);
        }
    }
}