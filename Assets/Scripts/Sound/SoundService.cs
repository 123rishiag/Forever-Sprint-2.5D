using System;
using UnityEngine;

namespace ServiceLocator.Sound
{
    public class SoundService
    {
        // Private Variables
        private SoundConfig soundConfig;
        private AudioSource bgmSource;
        private AudioSource sfxSource;

        public bool IsMute { get; private set; }
        private float bgmVolume;
        private float sfxVolume;

        public SoundService(SoundConfig _soundConfig, AudioSource _bgmSource, AudioSource _sfxSource)
        {
            // Setting Variables
            soundConfig = _soundConfig;
            bgmSource = _bgmSource;
            sfxSource = _sfxSource;

            IsMute = false;
            bgmVolume = bgmSource.volume;
            sfxVolume = sfxSource.volume;
        }

        public void Reset()
        {
            PlayBackgroundMusic(SoundType.BACKGROUND_MUSIC, true);
        }

        public void MuteGame()
        {
            IsMute = !IsMute; // Toggle mute
            SetVolume();
        }

        private void SetVolume()
        {
            bgmSource.volume = IsMute ? 0.0f : bgmVolume;
            sfxSource.volume = IsMute ? 0.0f : sfxVolume;
        }

        private void PlayBackgroundMusic(SoundType _soundType, bool _loopSound = true)
        {
            if (IsMute) return;

            AudioClip clip = GetSoundClip(_soundType);
            if (clip != null)
            {
                bgmSource.loop = _loopSound;
                bgmSource.clip = clip;
                bgmSource.Play();
            }
            else
                Debug.LogError("No Audio Clip selected.");
        }

        public void PlaySoundEffect(SoundType _soundType)
        {
            if (IsMute) return;

            AudioClip clip = GetSoundClip(_soundType);
            if (clip != null)
            {
                sfxSource.clip = clip;
                sfxSource.PlayOneShot(clip);
            }
            else
                Debug.LogError("No Audio Clip selected.");
        }

        private AudioClip GetSoundClip(SoundType _soundType)
        {
            SoundData sound = Array.Find(soundConfig.soundData, item => item.soundType == _soundType);
            if (sound.soundClip != null)
                return sound.soundClip;
            return null;
        }
    }
}