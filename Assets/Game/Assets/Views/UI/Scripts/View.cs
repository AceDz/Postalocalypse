using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public abstract class View<TScreen> : MonoBehaviour where TScreen : Screen
    {
        [SerializeField, BoxGroup("View")]
        string _screenAddress;

        protected TScreen _screen;

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        public void TestShow()
        {
            Show(null, null);
        }
#endif

        public void Show(System.Action onShow = null, System.Action onInstantiate = null)
        {
            if (_screen == null)
            {
                AssetsHelper.InstantiateObjectAsync<TScreen>(_screenAddress, (TScreen screen) =>
                {
                    _screen = screen;
                    OpenScreen(onInstantiate);
                });
            }
            else
            {
                onInstantiate?.Invoke();
                _screen.Open();
            }
        }

        public void Hide(System.Action onHide)
        {
            if (onHide != null)
            {
                _screen.OnClose += onHide;
            }
            _screen.Close();
        }

        private void OpenScreen(System.Action onInstantiate)
        {
            SetupScreen();
            onInstantiate?.Invoke();
            _screen.OnClose += ReleaseScreen;
            _screen.Open();
        }

        private void ReleaseScreen()
        {
            AssetsHelper.DestroyObject(_screen.gameObject);
            _screen = null;
        }

        protected abstract void SetupScreen();
    }
}