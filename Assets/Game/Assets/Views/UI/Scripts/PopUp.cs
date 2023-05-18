using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeamTheDream
{
    public class PopUp : Screen
    {
        [SerializeField]
        protected Image _overlay;

        [SerializeField]
        protected RectTransform _panel;

        private void Awake()
        {
            _overlay.color = new Color(0, 0, 0, 0);
            _panel.localScale = Vector2.zero;
        }

        public override bool Open()
        {
            if (!_isOpen)
            {
                _overlay.gameObject.SetActive(true);
                _panel.gameObject.SetActive(true);

                _isOpen = true;
                _audio.PlayPopupOpenSound();
                DOTween.Sequence()
                    .Append(_overlay.DOFade(0.8f, 0.2f))
                    .Join(_panel.DOScale(1f, 0.3f).SetEase(Ease.OutBack))
                    .OnComplete(() => {
                        SendScreenOpen();
                     });

                return true;
            }
            return false;
        }

        public override bool Close()
        {
            if (_isOpen)
            {
                _isOpen = false;

                _audio.PlayPopupCloseSound();
                DOTween.Sequence()
                    .Append(_overlay.DOFade(0, 0.3f))
                    .Join(_panel.DOScale(0, 0.3f).SetEase(Ease.InBack))
                    .OnComplete(() => {
                        _overlay.gameObject.SetActive(false);
                        _panel.gameObject.SetActive(false);
                        SendScreenClose();
                    });
                return true;
            }
            return false;
        }
    }
}