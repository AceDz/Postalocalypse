using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public static class SpriteRendererExtensions
    {
        public static void SetAlpha(this SpriteRenderer renderer, float alpha)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }
    }
}