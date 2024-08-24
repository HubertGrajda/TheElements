using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string mixerVolumeTag;
    
    private Slider _slider;
    
    private void Start()
    {
       _slider = GetComponent<Slider>();
        _slider.value = Managers.AudioManager.GetCurrentVolume(mixerVolumeTag);
    }
}
