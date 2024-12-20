using UnityEngine;
using System;
using System.Linq;
using _Scripts.Audio;
using UnityEngine.Audio;

namespace _Scripts.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private Sound[] sounds;
        [field: SerializeField] public AudioMixer Master {get; private set; }

        public const string MASTER_VOLUME_MIXER_TAG = "MasterVolume";
        public const string MUSIC_VOLUME_MIXER_TAG  = "MusicVolume";
        public const string SOUNDS_VOLUME_MIXER_TAG = "SoundsVolume";

        private AudioSource _soundsSource;
        
        protected override void Awake()
        {
            base.Awake();

            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.group;
            }

            var audioSources = GetComponents<AudioSource>();
            _soundsSource = audioSources.FirstOrDefault(source => source.outputAudioMixerGroup.name == "Sounds");
        }
        
        private void Start()
        {
            Instance.PlaySound("MainMenuMusic1");
        }
    
        public void PlaySound(string name)
        {
            var sound = Array.Find(sounds, sound => sound.name == name);
            
            if (sound == null) return;
            
            sound.source.Play();
        }
        
        public void PlaySound(AudioClip clip)
        {
            if(clip == null) return;
            
            _soundsSource.PlayOneShot(clip);
        }
    
        public void StopSound(string soundName)
        {
            var sound = Array.Find(sounds, sound => sound.name == soundName);
            
            if (sound == null) return;
            
            sound.source.Stop();
        }

        public float GetCurrentVolume(string mixer)
        {
            Master.GetFloat(mixer, out var currVolume);
            currVolume /= 20;
            currVolume = MathF.Pow(10, currVolume);
            return currVolume;
        }
    }
}