using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer[] _renderers;

        [Button, HideInEditorMode]
        public void Flash()
        {
            for (int i = 0; i < _renderers.Length; ++i)
            {
                _renderers[i].material.DOFloat(1f, "_FlashAmount", 0.1f).SetEase(Ease.OutFlash).SetLoops(2, LoopType.Yoyo);
            }
        }
    }
}