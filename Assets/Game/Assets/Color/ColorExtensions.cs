using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public static class ColorExtensions
    {
        public static void SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
        }

        public static bool SimilarTo(this Color color, Color compare, bool alpha = false, float threshold = 0.1f)
        {
            if (alpha)
            {
                if (threshold == 0)
                {
                    return color == compare;
                }
                Vector4 colorVector = new Vector4(color.r, color.g, color.b, color.a);
                Vector4 compareVector = new Vector4(compare.r, compare.g, compare.b, compare.a);

                return Vector4.Distance(colorVector, compareVector) <= threshold;
            }
            else
            {
                if (threshold == 0)
                {
                    return color.r == compare.r && color.g == compare.g && color.b == compare.b;
                }

                Vector3 colorVector = new Vector3(color.r, color.g, color.b);
                Vector3 compareVector = new Vector3(compare.r, compare.g, compare.b);

                return Vector3.Distance(colorVector, compareVector) <= threshold;
            }
        }
    }
}