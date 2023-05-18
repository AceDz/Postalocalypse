using System;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class NonPlayerCharacter : SerializedWorldActor, IDeliveryInteractable, ITalkable
    {
        [SerializeField, BoxGroup("Talk Icon")] private GameObject _talkIcon;
        
        [SerializeField, BoxGroup("Dialogue")] private AudioClip _charVoiceClip;

        [SerializeField, BoxGroup("Dialogue")] private RectTransform _dialoguePanel;

        [SerializeField, BoxGroup("Dialogue")] private TextVisibilityAnimation _dialogueText;

        [OdinSerialize, BoxGroup("Dialogue")] private Conversation _firstConversation;
        [OdinSerialize, BoxGroup("Dialogue")] private Conversation _foreverConversation;

        [OdinSerialize, BoxGroup("Locked Dialogue")]
        private Conversation _lockedConversation;

        [OdinSerialize, BoxGroup("Locked Dialogue")]
        private UsbKeyItem _requiredUsbKeyItem;

        private bool _isInteracting;
        private bool _isFirstConversationCompleted;
        private bool _isLockedConversationCompleted;

        private Conversation _currentConversation;
        private CoroutineHandle _conversationCoroutine;

        private PlayerMovementController _playerMovementController;
        
        private AudioSource _talkAudioSource;
        private AudioManager _audioManager;

        private DialogueController _dialogueController;
        
        private bool IsLockedConversationAvailable => _lockedConversation != null &&
                                                      _requiredUsbKeyItem != null &&
                                                      _requiredUsbKeyItem.PickedAmount >=
                                                      _requiredUsbKeyItem.MaxItemAmount;

        public event System.Action OnFirstConversationEnd;

        private void Start()
        {
            _dialogueText.OnWriteChar += DialogueText_OnWriteChar;
            _audioManager = FindObjectOfType<AudioManager>();
            _dialogueController = new DialogueController(_dialoguePanel, _dialogueText);
        }

        private void UpdateConversation()
        {
            if (Input.GetButtonDown("Interact"))
            {
                _currentConversation.Continue();
            }
        }

        private void DialogueText_OnWriteChar()
        {
            // if (_talkAudioSource == null)
            // {
            //     _audioManager = FindObjectOfType<AudioManager>();
            //     _talkAudioSource = _audioManager.PlaySound(_charVoiceClip);
            //     _talkAudioSource.pitch = Random.Range(1f, 1.5f);
            // }
            // else
            // {
            //     _audioManager.PlaySound(_talkAudioSource, _charVoiceClip);
            //     _talkAudioSource.pitch = Random.Range(1f, 1.5f);
            // }
        }

        public void CanInteract(PlayerMovementController player, bool can)
        {
            _talkIcon.SetActive(can);

            if (!can && _isInteracting)
            {
                EndInteraction(false);
            }
        }

        public void Interact(PlayerMovementController player)
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                _playerMovementController = player;
                _playerMovementController.Stop();
                _talkIcon.SetActive(false);

                if (IsLockedConversationAvailable)
                {
                    _currentConversation = _lockedConversation;
                    _currentConversation.Begin();
                    _currentConversation.OnComplete += LockedConversation_OnComplete;
                    _conversationCoroutine = Timing.CallContinuously(Mathf.Infinity, UpdateConversation);
                }
                else if (!_isFirstConversationCompleted && _firstConversation != null)
                {
                    _currentConversation = _firstConversation;
                    _currentConversation.Begin();
                    _currentConversation.OnComplete += FirstConversation_OnComplete;

                    _conversationCoroutine = Timing.CallContinuously(Mathf.Infinity, UpdateConversation);
                }
                else if (_foreverConversation != null)
                {
                    _currentConversation = _foreverConversation;
                    _currentConversation.Begin();
                    _currentConversation.OnComplete += ForeverConversation_OnComplete;

                    _conversationCoroutine = Timing.CallContinuously(Mathf.Infinity, UpdateConversation);
                }
            }
        }
        
        private void LockedConversation_OnComplete()
        {
            _isInteracting = false;
            _playerMovementController.Resume();
            _currentConversation.OnComplete -= LockedConversation_OnComplete;
            _currentConversation = null;

            _isLockedConversationCompleted = true;
            _audioManager.PlayTakeFragmentSfx();

            Timing.KillCoroutines(_conversationCoroutine);
        }

        private void FirstConversation_OnComplete()
        {
            _isInteracting = false;
            _playerMovementController.Resume();
            _currentConversation.OnComplete -= FirstConversation_OnComplete;
            _currentConversation = null;

            _isFirstConversationCompleted = true;
            _audioManager.PlayTakeFragmentSfx();

            Timing.KillCoroutines(_conversationCoroutine);
            
            OnFirstConversationEnd?.Invoke();
        }

        private void ForeverConversation_OnComplete()
        {
            _isInteracting = false;
            _playerMovementController.Resume();
            _currentConversation.OnComplete -= ForeverConversation_OnComplete;
            _currentConversation = null;

            Timing.KillCoroutines(_conversationCoroutine);
        }

        public void BeginTalk(string text, Action onComplete)
        {
            _dialogueController.BeginTalk(text, onComplete);
        }

        public void EndTalk(Action onComplete = null)
        {
            _dialogueController.EndTalk(onComplete);
        }

        public void ContinueTalk()
        {
            _dialogueController.ContinueTalk();
        }

        private void EndInteraction(bool canInteract)
        {
            if (_isInteracting)
            {
                _isInteracting = false;

                Timing.KillCoroutines(_conversationCoroutine);
                EndTalk();
                _talkIcon.gameObject.SetActive(canInteract);
            }
        }
    }
}