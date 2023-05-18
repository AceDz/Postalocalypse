using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TeamTheDream
{
    public class TextFadeAnimation : MonoBehaviour
    {
        TMP_Text _text;

        Tween _tween;

        private TMP_Text Text {
            get {
                if (_text == null)
                {
                    _text = GetComponent<TMP_Text>();
                }
                return _text;
            }
        }

        private void OnDestroy()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }

        private void OnDisable()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }

        private void OnEnable()
        {
            _tween = Text.DOFade(0.5f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}