using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream.Delivery
{
    [ExecuteInEditMode]
    public class BoxCollider2DAutosize : MonoBehaviour
    {
#if UNITY_EDITOR
        private SpriteRenderer _renderer;
        private BoxCollider2D _collider;

        
        void Update()
        {
            if (_renderer == null|| _collider == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
                _collider = GetComponent<BoxCollider2D>();
                Resize();
            }
            else if (_renderer.size != _collider.size)
            {
                Resize();
            }
        }

        [Button]
        private void Resize()
        {
            _collider.offset = _renderer.size / 2f;
            _collider.size = _renderer.size;
        }
#endif
    }
}