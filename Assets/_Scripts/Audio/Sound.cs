using UnityEngine.Audio;
using UnityEngine;

namespace _Scripts.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup group;

        [Range(0.01f, 1f)]
        public float volume;
    
        [Range(0.1f, 3)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
}