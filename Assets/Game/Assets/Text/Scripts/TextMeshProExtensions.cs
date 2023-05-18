using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TeamTheDream
{
    public static class TextMeshProExtensions
    {
        public static void SetAlpha(this TMP_Text text, float alpha)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }
}
