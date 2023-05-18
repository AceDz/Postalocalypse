using System;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using TeamTheDream.Delivery;
using UnityEngine;

namespace TeamTheDream
{
    public class PlayerInteract : SerializedMonoBehaviour , ITalkable
    {
        [SerializeField, BoxGroup("Player")]
        private PlayerMovementController _playerMovementController;
        
        [SerializeField, BoxGroup("Player")]
        private PlayerBridge _playerBridge;
        
        [SerializeField, BoxGroup("Dialogue")]
        private RectTransform _dialoguePanel;

        [SerializeField, BoxGroup("Dialogue")]
        private TextVisibilityAnimation _dialogueText;
        
        [SerializeField, BoxGroup("Audio")] 
        private AudioManager _audio;
        
        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        protected IDeliveryInteractable _activeInteractable;
        
        private DialogueController _dialogueController;

        private void Start()
        {
            _playerBridge.BindPlayer(this);
            _dialogueController = new DialogueController(_dialoguePanel, _dialogueText);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact"))
            {
                UpdateInteract();
            }
        }

        private void UpdateInteract()
        {
            if (_activeInteractable != null)
            {
                _activeInteractable.Interact(_playerMovementController);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var interactable = collision.GetComponent<IDeliveryInteractable>();
            if (interactable != null)
            {
                _activeInteractable = interactable;
                _activeInteractable.CanInteract(_playerMovementController, true);
            }

            var pickable = collision.GetComponent<IPickable>();
            if (pickable != null)
            {
                _audio.PlayTakeFragmentSfx();
                pickable.Pick();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var interactable = collision.GetComponent<IDeliveryInteractable>();
            if (interactable != null)
            {
                interactable.CanInteract(_playerMovementController, false);
                _activeInteractable = null;
            }
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
    }
}