using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}