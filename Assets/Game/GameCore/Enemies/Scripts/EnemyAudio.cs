using UnityEngine;

namespace TeamTheDream.Delivery
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemyAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource; 
        [SerializeField] private AudioClip _attackAudioClip;
        [SerializeField] private AudioClip[] _hurtAudioClips;
        [SerializeField] private AudioClip[] _deadAudioClips;

        public void PlayAttack()
        {
            _audioSource.PlayOneShot(_attackAudioClip);
        }
        
        public void PlayHurt()
        {
            _audioSource.PlayOneShot(_hurtAudioClips[Random.Range(0, _hurtAudioClips.Length)]);
        }
        
        public void PlayDead()
        {
            _audioSource.PlayOneShot(_deadAudioClips[Random.Range(0, _deadAudioClips.Length)]);
        }
    }
}