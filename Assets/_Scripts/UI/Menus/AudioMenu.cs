using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class AudioMenu : Menu
   {
      [SerializeField] private Button backBtn;

      [SerializeField] private Slider masterVolumeSlider;
      [SerializeField] private Slider musicVolumeSlider;
      [SerializeField] private Slider soundsVolumeSlider;

      private AudioManager _audioManager;
   
      protected override void Start()
      {
         base.Start();
         _audioManager = AudioManager.Instance;
      }

      public override void Open()
      {
         base.Open();
      
         masterVolumeSlider.value = _audioManager.GetCurrentVolume(AudioManager.MASTER_VOLUME_MIXER_TAG);
         musicVolumeSlider.value  = _audioManager.GetCurrentVolume(AudioManager.MUSIC_VOLUME_MIXER_TAG);
         soundsVolumeSlider.value = _audioManager.GetCurrentVolume(AudioManager.SOUNDS_VOLUME_MIXER_TAG);
      }
   
      protected override void InitUIElements()
      {
         backBtn.onClick.AddListener(OnBackButtonClicked);

         masterVolumeSlider.onValueChanged.AddListener(volume =>
            OnVolumeSliderValueChanged(AudioManager.MASTER_VOLUME_MIXER_TAG, volume));
      
         musicVolumeSlider.onValueChanged.AddListener(volume =>
            OnVolumeSliderValueChanged(AudioManager.MUSIC_VOLUME_MIXER_TAG, volume));
      
         soundsVolumeSlider.onValueChanged.AddListener(volume =>
            OnVolumeSliderValueChanged(AudioManager.SOUNDS_VOLUME_MIXER_TAG, volume));
      }
   
      private void OnVolumeSliderValueChanged(string mixerTag, float volume)
      {
         _audioManager.Master.SetFloat(mixerTag, Mathf.Log10(volume) * 20);
      }

      private void OnDestroy()
      {
         backBtn.onClick.RemoveAllListeners();
         masterVolumeSlider.onValueChanged.RemoveAllListeners();
         musicVolumeSlider.onValueChanged.RemoveAllListeners();
         soundsVolumeSlider.onValueChanged.RemoveAllListeners();
      }
   }
}
