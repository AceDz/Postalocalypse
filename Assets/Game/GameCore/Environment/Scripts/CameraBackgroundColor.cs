using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class CameraBackgroundColor : MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraTransform;

        [SerializeField]
        private SpriteRenderer _background;

        [SerializeField]
        private ParticleSystem _particles;

        [SerializeField]
        private float _maxYPosition;

        [SerializeField]
        private float _minYPosition;

        [SerializeField]
        private Gradient _gradient;

        [SerializeField]
        private Color _backgroundDeathColor = Color.white;

        [SerializeField]
        private Color _particlesDeathColor = Color.white;

        private float _lastYPos = Mathf.Infinity;

        private bool _active;

        private void Update()
        {
            if (_active && _cameraTransform.position.y != _lastYPos)
            {
                _lastYPos = _cameraTransform.position.y;
                _background.color = _gradient.Evaluate(Mathf.Clamp01((_lastYPos - _minYPosition) / (_maxYPosition - _minYPosition)));
            }
        }

        public void Init()
        {
            _active = true;
        }

        public void Stop()
        {
            _active = false;
        }

        public Tween SetFirstColor(float duration)
        {
            return _background.DOColor(_gradient.Evaluate(0), duration);
        }

        public Sequence DODeathColors(float duration)
        {
            return DOTween.Sequence()
                .Append(_background.DOColor(_backgroundDeathColor, duration))
                .Join(_particles.DOStartColor(_particlesDeathColor, duration))
                .Join(_particles.DOParticlesColor(_particlesDeathColor, duration));
        }
    }
}