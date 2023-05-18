using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [System.Serializable]
    public struct DialogueLine
    {
        [OdinSerialize]
        private ITalkable _speaker;

        [SerializeField]
        private DialogueText _text;

        [SerializeField] 
        private IUnlockable _unlockable;

        public void BeginTalk(System.Action onComplete)
        {
            _speaker.BeginTalk(_text.Text, onComplete);
        }

        public void EndTalk(System.Action onComplete)
        {
            _speaker.EndTalk(onComplete);
            if(_unlockable != null)
                _unlockable.Unlock();
        }

        public void ContinueTalk()
        {
            _speaker.ContinueTalk();
        }
    }
}
