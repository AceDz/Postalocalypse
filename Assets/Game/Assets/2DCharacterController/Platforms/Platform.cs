using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class Platform : MonoBehaviour
    {
        [SerializeField]
        private PlatformEffector2D _effector;

        [SerializeField]
        private Ladder _hanger;

        public Ladder Hanger => _hanger;

        public PlatformEffector2D Effector => _effector;
    }
}