using _Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : Menu
{
   [SerializeField] private Button backBtn;

   [SerializeField] private Slider masterVolumeSlider;
   [SerializeField] private Slider musicVolumeSlider;
   [SerializeField] private Slider soundsVolumeSlider;

   public override void Open()
   {
      base.Open();
      
      masterVolumeSlider.value = Managers.AudioManager.GetCurrentVolume(AudioManager.MASTER_VOLUME_MIXER_TAG);
      musicVolumeSlider.value  = Managers.AudioManager.GetCurrentVolume(AudioManager.MUSIC_VOLUME_MIXER_TAG);
      soundsVolumeSlider.value = Managers.AudioManager.GetCurrentVolume(AudioManager.SOUNDS_VOLUME_MIXER_TAG);
   }
   
   protected override void InitUIElements()
   {
      backBtn.onClick.AddListener(OnBackButtonClicked);

      masterVolumeSlider.onValueChanged.AddListener((volume) =>
         OnVolumeSliderValueChanged(AudioManager.MASTER_VOLUME_MIXER_TAG, volume));
      
      musicVolumeSlider.onValueChanged.AddListener((volume) =>
         OnVolumeSliderValueChanged(AudioManager.MUSIC_VOLUME_MIXER_TAG, volume));
      
      soundsVolumeSlider.onValueChanged.AddListener((volume) =>
         OnVolumeSliderValueChanged(AudioManager.SOUNDS_VOLUME_MIXER_TAG, volume));
   }
   
   private void OnVolumeSliderValueChanged(string mixerTag, float volume)
   {
      Managers.AudioManager.master.SetFloat(mixerTag, Mathf.Log10(volume) * 20);
   }

}
