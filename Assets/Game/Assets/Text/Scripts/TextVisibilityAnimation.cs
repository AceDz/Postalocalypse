using TMPro;
using UnityEngine;
using MEC;

namespace TeamTheDream
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextVisibilityAnimation : MonoBehaviour
    {
        private TMP_Text _text;

        [SerializeField]
        private bool _playOnStart = true;

        [SerializeField]
        private float _delayScale = 1f;

        [SerializeField]
        private float _breakDelayScale = 1f;

        [SerializeField]
        private float _firstDelay = 0.1f;

        CoroutineHandle _showCoroutine;

        private bool _isAnimating;
        private int _suspendedPoints;

        public event System.Action OnWriteChar;
        public event System.Action OnComplete;

        public bool IsComplete => _text != null && _text.textInfo != null && _text.maxVisibleCharacters == _text.textInfo.characterCount;

        public TMP_Text Text {
            get {
                if (_text == null)
                {
                    _text = GetComponent<TMP_Text>();
                }
                return _text;
            }
        }

        private void Start()
        {
            if (_playOnStart)
            {
                Play();
            }
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_showCoroutine);
        }

        public void Play()
        {
            Text.maxVisibleCharacters = 0;
            _showCoroutine = Timing.CallDelayed(_firstDelay, ShowNext);
            _isAnimating = true;
        }

        public void Play(string text)
        {
            Text.text = text;
            Text.maxVisibleCharacters = 0;
            _showCoroutine = Timing.CallDelayed(_firstDelay, ShowNext);
            _isAnimating = true;
        }

        public void Continue()
        {
            if (!_isAnimating)
            {
                _showCoroutine = Timing.CallDelayed(_firstDelay, ShowNext);
                _isAnimating = true;
            }
        }

        public void AppendText(string text)
        {
            Text.text += text;
            if (!_isAnimating)
            {
                _showCoroutine = Timing.CallDelayed(_firstDelay, ShowNext);
                _isAnimating = true;
            }
        }

        public void AppendLine()
        {
            Text.text += "\n";
        }

        public void AppendLine(string text)
        {
            Text.text += "\n" + text;
            if (!_isAnimating)
            {
                _showCoroutine = Timing.CallDelayed(_firstDelay, ShowNext);
                _isAnimating = true;
            }
        }

        public void Complete()
        {
            Timing.KillCoroutines(_showCoroutine);
            Text.maxVisibleCharacters = Text.textInfo.characterCount;
            _isAnimating = false;

            OnComplete?.Invoke();
        }

        private void ShowNext()
        {
            if (Text.maxVisibleCharacters < Text.textInfo.characterCount)
            {
                switch (Text.textInfo.characterInfo[Text.maxVisibleCharacters].character)
                {
                    case ',':
                        if (_delayScale == 0)
                        {
                            ++Text.maxVisibleCharacters;
                            ShowNext();
                            return;
                        }
                        else
                        {
                            _showCoroutine = Timing.CallDelayed(0.15f * _delayScale, ShowNext);
                        }
                        break;

                    case ';':
                        if (_delayScale == 0)
                        {
                            ++Text.maxVisibleCharacters;
                            ShowNext();
                            return;
                        }
                        else
                        {
                            _showCoroutine = Timing.CallDelayed(0.15f * _delayScale, ShowNext);
                        }
                        break;

                    case '.':
                        if (_delayScale == 0)
                        {
                            ++Text.maxVisibleCharacters;
                            ShowNext();
                            return;
                        }
                        else
                        {
                            if (_suspendedPoints > 0)
                            {
                                if (_suspendedPoints == 1)
                                {
                                    _showCoroutine = Timing.CallDelayed(0.2f * _delayScale, ShowNext);
                                }
                                else
                                {
                                    _showCoroutine = Timing.CallDelayed(0.1f * _delayScale, ShowNext);
                                }
                                --_suspendedPoints;
                            }
                            else
                            {
                                int i = Text.maxVisibleCharacters + 1;
                                for (; i < Text.textInfo.characterCount; ++i)
                                {
                                    if (Text.textInfo.characterInfo[i].character != '.')
                                    {
                                        break;
                                    }
                                    ++_suspendedPoints;
                                }
                                if (_suspendedPoints >= 1)
                                {
                                    _showCoroutine = Timing.CallDelayed(0.1f * _delayScale, ShowNext);
                                }
                                else
                                {
                                    _suspendedPoints = 0;
                                    _showCoroutine = Timing.CallDelayed(0.4f * _delayScale, ShowNext);
                                }
                            }
                        }
                        break;

                        case '\n':
                            if (_breakDelayScale == 0)
                            {
                                ++Text.maxVisibleCharacters;
                                ShowNext();
                                return;
                            }
                            else
                            {
                                _showCoroutine = Timing.CallDelayed(0.25f * _breakDelayScale, ShowNext);
                            }
                        break;

                    default:
                        if (_delayScale == 0)
                        {
                            ++Text.maxVisibleCharacters;
                            ShowNext();
                            return;
                        }
                        else
                        {
                            _showCoroutine = Timing.CallDelayed(Random.Range(0.05f, 0.01f) * _delayScale, ShowNext, gameObject);
                        }
                        break;
                }
                OnWriteChar?.Invoke();
                ++Text.maxVisibleCharacters;
            }
            else
            {
                Complete();
            }
        }

        public void Clear()
        {
            if (_isAnimating)
            {
                Timing.KillCoroutines(_showCoroutine);
                _isAnimating = false;
            }
            Text.text = string.Empty;
        }
    }
}