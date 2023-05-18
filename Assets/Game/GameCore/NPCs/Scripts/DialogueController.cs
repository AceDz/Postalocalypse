using System;
using DG.Tweening;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    public class DialogueController
    {
        private RectTransform _dialoguePanel;
        private TextVisibilityAnimation _dialogueText;
        
        private int _currentIndex;
        private string[] _texts;
        private Action _onCompleteCurrentLine;
        private Tween _showDialogueTween;

        public DialogueController(RectTransform dialoguePanel, TextVisibilityAnimation dialogueText)
        {
            _dialoguePanel = dialoguePanel;
            _dialogueText = dialogueText;
        }
        
        public void BeginTalk(string text, Action onComplete)
        {
            _currentIndex = 0;
            _texts = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            _onCompleteCurrentLine = onComplete;
            _dialogueText.OnComplete += CheckComplete;
            _dialoguePanel.SetScale(0);
            _dialoguePanel.gameObject.SetActive(true);


            _showDialogueTween?.Kill();
            _showDialogueTween = _dialoguePanel.DOScale(1f, 0.1f).OnComplete(() =>
            {
                _dialogueText.Play(_texts[_currentIndex]);
            });
        }

        private void CheckComplete()
        {
            if (_currentIndex + 1 < _texts.Length)
            {
                return;
            }
            
            _dialogueText.OnComplete -= CheckComplete;
            _onCompleteCurrentLine?.Invoke();
        }
        
        public void EndTalk(Action onComplete = null)
        {
            _showDialogueTween?.Kill();
            _showDialogueTween = _dialoguePanel.DOScale(0, 0.1f).OnComplete(() =>
            {
                _dialoguePanel.gameObject.SetActive(false);
                _dialogueText.Clear();
                
                _currentIndex = 0;
                _texts = null;
                _onCompleteCurrentLine = null;
                
                onComplete?.Invoke();
            });
        }

        public void ContinueTalk()
        {
            if (!_dialogueText.IsComplete)
            {
                _dialogueText.Complete();
                return;
            }
            
            
            ++_currentIndex;
            if (_currentIndex < _texts.Length)
            {
                _dialogueText.Play(_texts[_currentIndex]);
            }
        }
    }
}