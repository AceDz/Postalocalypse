using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [System.Serializable]
    public class Conversation
    {
        [OdinSerialize]
        private DialogueLine[] _lines;

        private int _currentLine;
        private bool _currentLineComplete;

        public event System.Action OnComplete;

        public void Begin()
        {
            _currentLine = 0;
            _currentLineComplete = false;
            _lines[_currentLine].BeginTalk(Complete);
        }

        public void Continue()
        {
            if (_currentLineComplete)
            {
                _lines[_currentLine].EndTalk(NextLine);
            }
            else
            {
                _lines[_currentLine].ContinueTalk();
            }
        }

        private void Complete()
        {
            _currentLineComplete = true;
        }

        private void NextLine()
        {
            ++_currentLine;
            if (_currentLine < _lines.Length)
            {
                _currentLineComplete = false;
                _lines[_currentLine].BeginTalk(Complete);
            }
            else
            {
                OnComplete?.Invoke();
            }
        }
    }

}
