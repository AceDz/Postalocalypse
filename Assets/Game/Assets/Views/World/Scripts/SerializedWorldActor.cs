using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class SerializedWorldActor : SerializedMonoBehaviour
    {
        public Transform Transform => transform; // Unity caches transform
    }
}