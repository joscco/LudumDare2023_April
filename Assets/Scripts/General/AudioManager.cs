using DG.Tweening;
using UnityEngine;

namespace General
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        
        public AudioSource EffectsSource;
        public AudioSource MusicSource;

        public AudioClip mainMusic;
        public AudioClip[] blubSounds;
        public AudioClip[] moveSounds;

        private void Start()
        {
            instance = this;
        }

        public void SetMusicVolume(float value)
        {
            MusicSource.volume = value;
        }
        
        public void SetSFXVolume(float value)
        {
            EffectsSource.volume = value;
        }
        
        public void PlayBlub()
        {
            EffectsSource.clip = blubSounds[Random.Range(0, blubSounds.Length)];
            EffectsSource.Play();
        }

        public void PlayMoveSound()
        {
            EffectsSource.clip = moveSounds[Random.Range(0, moveSounds.Length)];
            EffectsSource.Play();
        }

        // Play a single clip through the music source.
        public void PlayMusic()
        {
            if (!MusicSource.isPlaying)
            {
                MusicSource.clip = mainMusic;
                MusicSource.Play();
                MusicSource.volume = 0;
                MusicSource.DOFade(1, 2f);

            }
        }
        
        public void StopMusic()
        {
            MusicSource.Stop();
        }
    }
}