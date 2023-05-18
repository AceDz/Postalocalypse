using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TeamTheDream
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField, BoxGroup("Music")]
        private AudioClip _titleMusicClip;

        [SerializeField, BoxGroup("Music")]
        private AudioClip _gameMusicClip;

        [SerializeField, BoxGroup("Music")]
        private AudioClip _bossMusicClip;

        [SerializeField, BoxGroup("Music")]
        private AudioClip _gameOverClip;

        [SerializeField, BoxGroup("Music")]
        private AudioClip _gameWinClip;

        [SerializeField, BoxGroup("Background Sfx")]
        private AudioClip _shotSfxClip;

        [SerializeField, BoxGroup("Background Sfx")]
        private AudioClip _projectileHitsClip;

        [SerializeField, BoxGroup("UI Sfx")]
        private AudioClip _selectClip;

        [SerializeField, BoxGroup("UI Sfx")]
        private AudioClip _popupOpenClip;

        [SerializeField, BoxGroup("UI Sfx")]
        private AudioClip _popupCloseClip;

        [SerializeField, BoxGroup("UI Sfx")]
        private AudioClip _showTextClip;

        [SerializeField, BoxGroup("UI Sfx")]
        private AudioClip _startGameClip;

        [SerializeField, BoxGroup("Voices Sfx")]
        private AudioClip _pinkiePunkiNinjaKittieVoiceClip;

        [SerializeField, BoxGroup("Voices Sfx")]
        private AudioClip _moneyMoneySystemDoggyVoiceClip;

        [SerializeField, BoxGroup("Effects Sfx")]
        private AudioClip _takeFragmentSfxClip;

        [SerializeField, BoxGroup("Player Sfx")]
        private AudioClip _playerSmokeSfxClip;

        [SerializeField, BoxGroup("Player Sfx")]
        private AudioClip _playerJumpSfxClip;

        [SerializeField, BoxGroup("Player Sfx")]
        private AudioClip _playerLandSfxClip;

        [SerializeField, BoxGroup("Player Sfx")]
        private AudioClip _playerStepSfxClip;

        private AudioSource _musicSource;
        private AudioSource _ambienceSoundSource;
        private List<AudioSource> _audioSources;

        protected AudioSource GetAudioSource()
        {
            for (int i = 0; i < _audioSources.Count; ++i)
            {
                if (!_audioSources[i].isPlaying)
                {
                    return _audioSources[i];
                }
            }

            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.loop = false;
            newSource.playOnAwake = false;
            _audioSources.Add(newSource);
            return newSource;
        }

        private void Awake()
        {
            Setup();
            PlayGameMusic();
        }

        public void Setup()
        {
            //_container = container;
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            _ambienceSoundSource = gameObject.AddComponent<AudioSource>();
            _ambienceSoundSource.clip = _shotSfxClip;
            _ambienceSoundSource.loop = true;
            _ambienceSoundSource.playOnAwake = false;

            _audioSources = new List<AudioSource>();
        }

        public void PlaySound(AudioSource audioSource, AudioClip sound)
        {
            audioSource.pitch = 1f;
            audioSource.clip = sound;
            audioSource.Play();
        }

        public AudioSource PlaySound(AudioClip sound)
        {
            AudioSource audioSource = GetAudioSource();
            audioSource.pitch = 1f;
            audioSource.clip = sound;
            audioSource.Play();
            return audioSource;
        }
        
        public AudioSource PlaySound(AudioClip sound, float pitch)
        {
            AudioSource audioSource = GetAudioSource();
            audioSource.pitch = pitch;
            audioSource.clip = sound;
            audioSource.Play();
            return audioSource;
        }

        public void Release()
        {
            //_container = null;
            _musicSource = null;
            _audioSources = null;
        }

        public void PlayMusic()
        {
            if (!_musicSource.isPlaying)
            {
                _musicSource.Play();
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.volume = 0.6f;
            _musicSource.Play();
        }

        public void PlayMusic(AudioClip clip, float fade)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
            _musicSource.volume = 0;
            _musicSource.DOFade(0.6f, fade);
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void StopMusic(float duration)
        {
            _musicSource.DOFade(0, duration).OnComplete(StopMusic);
        }

        public void PlayTitleMusic()
        {
            PlayMusic(_titleMusicClip, 0.5f);
        }

        public void PlayGameMusic()
        {
            PlayMusic(_gameMusicClip);
        }
        internal void PlayGameOverMusic()
        {
            PlayMusic(_gameOverClip);
        }

        public AudioSource PlayPopupOpenSound()
        {
            return PlaySound(_popupOpenClip);
        }

        public AudioSource PlayPopupCloseSound()
        {
            return PlaySound(_popupCloseClip);
        }

        public AudioSource PlaySelectSound()
        {
            return PlaySound(_selectClip);
        }
        

        public AudioSource PlayTakeFragmentSfx()
        {
            return PlaySound(_takeFragmentSfxClip);
        }

        public AudioSource PlayShowTextSound()
        {
            AudioSource audioSource = PlaySound(_showTextClip);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            return audioSource;
        }

        public AudioSource PlayStartGameSound()
        {
            return PlaySound(_startGameClip);
        }

        public AudioSource PlayPlayerSmokeSfx()
        {
            return PlaySound(_playerSmokeSfxClip);
        }

        public AudioSource PlayPlayerJumpSfx()
        {
            return PlaySound(_playerJumpSfxClip);
        }

        public AudioSource PlayPlayerLandSfx()
        {
            return PlaySound(_playerLandSfxClip);
        }

        public AudioSource PlayPlayerStepSfx()
        {
            return PlaySound(_playerStepSfxClip);
        }

        public AudioSource PlayProjectileHitsSfx()
        {
            return PlaySound(_projectileHitsClip, Random.Range(0.9f, 1.1f));
        }

        public AudioSource PlayShotSfx(float sqrForce)
        {
            Debug.Log("sqrForce  => "+sqrForce);
            return PlaySound(_shotSfxClip, 0.5f + (0.5f * sqrForce / 200f));
        }


        public void StopAmbienceSound()
        {
            _ambienceSoundSource.DOFade(0, 2f)
                    .OnComplete(_ambienceSoundSource.Stop);
        }
    }
}

