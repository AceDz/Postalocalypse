using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TeamTheDream
{
    public class GraphicFadeAnimation : MonoBehaviour
    {
        Graphic _graphic;

        Tween _tween;

        private Graphic Graphic {
            get {
                if (_graphic == null)
                {
                    GetComponent<Graphic>();
                }
                return _graphic;
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
            _tween = Graphic.DOFade(0.5f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}