using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    [System.Serializable]
    public struct Attack
    {
        [SerializeField]
        private float _duration;

        [SerializeField]
        private float _castDuration;

        [SerializeField]
        private float _damageDuration;

        [SerializeField]
        private Damage _damage;

        public float Duration => _duration;
        public float CastDuration => _castDuration;
        public float DamageDuration => _damageDuration;
        public Damage Damage => _damage;
    }
}