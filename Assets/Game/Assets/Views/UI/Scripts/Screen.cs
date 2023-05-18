using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField]
        protected AudioManager _audio;

        protected bool _isOpen;

        public AudioManager Audio => _audio;
        public bool IsOpen => _isOpen;

        public event System.Action OnOpen;
        public event System.Action OnClose;

        public void Show()
        {
            Open();
        }

        public void Hide()
        {
            Close();
        }

        public virtual bool Open()
        {
            if (!_isOpen)
            {
                _isOpen = true;

                gameObject.SetActive(true);
                SendScreenOpen();
                return true;
            }
            return false;
        }

        public virtual bool Close()
        {
            if (_isOpen)
            {
                _isOpen = false;

                gameObject.SetActive(false);
                SendScreenClose();
                return true;
            }
            return false;
        }

        protected void SendScreenOpen()
        {
            OnOpen?.Invoke();
        }

        protected void SendScreenClose()
        {
            OnClose?.Invoke();
        }
    }
}