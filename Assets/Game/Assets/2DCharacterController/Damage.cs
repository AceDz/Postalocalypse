using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    [System.Serializable]
    public class Damage
    {
        // Types (%physycal, %fire, %ice, etc)

        [SerializeField]
        private int _amount;

        [SerializeField]
        private float _pushForce;

        [SerializeField]
        private float _stuntDuration;

        [SerializeField]
        private bool _ignoreInvulnerability;

        public int Amount => _amount;

        public float PushForce => _pushForce;

        public float StuntDuration => _stuntDuration;

        public bool IgnoreInvulnerability => _ignoreInvulnerability;

        public Damage(int amount, float pushForce = 0, float stuntDuration = 0 , bool ignoreInvulnerabilty = false)
        {
            _amount = amount;
            _pushForce = pushForce;
            _stuntDuration = stuntDuration;
            _ignoreInvulnerability = ignoreInvulnerabilty;
        }
    }
}