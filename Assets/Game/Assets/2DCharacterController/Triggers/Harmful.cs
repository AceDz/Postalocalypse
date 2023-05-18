using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public class Harmful : WorldActor, ITriggerable
    {
        [SerializeField]
        private Damage _damage;

        [SerializeField]
        private float _loopTime;

        CoroutineHandle _damageCoroutine;

        private PlayerController _player;

        public event Action<PlayerController> OnTrigger;

        public void EnterTrigger(PlayerController player)
        {
            _player = player;
            Damage();
            if (_loopTime > 0)
            {
                _damageCoroutine = Timing.CallPeriodically(_loopTime, Mathf.Infinity, Damage);
            }
        }

        public void ExitTrigger()
        {
            _player = null;
            Timing.KillCoroutines(_damageCoroutine);
        }

        private void Damage()
        {
            _player.TakeDamage(_damage, Transform);
            OnTrigger?.Invoke(_player);
        }
    }
}