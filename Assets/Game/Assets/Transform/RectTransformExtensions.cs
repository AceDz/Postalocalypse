using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetAnchoredPositionX(this RectTransform t, float x)
    {
        Vector2 position = t.anchoredPosition;
        position = new Vector2(x, position.y);
        t.anchoredPosition = position;
    }

    public static void SetAnchoredPositionY(this RectTransform t, float y)
    {
        Vector2 position = t.anchoredPosition;
        position = new Vector2(position.x, y);
        t.anchoredPosition = position;
    }

    public static void SetSizeDelta(this RectTransform t, float size)
    {
        t.sizeDelta = new Vector2(size, size);
    }
}
