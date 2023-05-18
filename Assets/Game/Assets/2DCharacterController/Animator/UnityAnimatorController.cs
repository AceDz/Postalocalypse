using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    [System.Serializable]
    public class UnityAnimatorController : IAnimatorController
    {
        private const string WALKING_PARAM = "isWalking";
        private const string RUNNING_PARAM = "isRunning";
        private const string JUMPING_PARAM = "isJumping";
        private const string FALLING_PARAM = "isFalling";
        private const string CROUCH_PARAM = "isCrouch";
        private const string CLIMBING_PARAM = "isClimbing";
        private const string HANGING_PARAM = "isHanging";
        private const string LADDING_PARAM = "isLadding";
        private const string ATTACKING_PARAM = "isAttacking";
        private const string HURT_PARAM = "hurt";
        private const string SPEED_PARAM = "speed";
        private const string ATTACKCOMBO_PARAM = "attackCombo";

        [SerializeField]
        private Animator _animator;

        public void BeginWalk()
        {
            _animator.SetBool(WALKING_PARAM, true);
        }

        public void EndWalk()
        {
            _animator.SetBool(WALKING_PARAM, false);
        }

        public void BeginRun()
        {
            _animator.SetBool(RUNNING_PARAM, true);
        }

        public void EndRun()
        {
            _animator.SetBool(RUNNING_PARAM, false);
        }

        public void BeginJump()
        {
            _animator.SetBool(JUMPING_PARAM, true);
        }

        public void EndJump()
        {
            _animator.SetBool(JUMPING_PARAM, false);
        }

        public void BeginFall()
        {
            _animator.SetBool(FALLING_PARAM, true);
        }

        public void EndFall()
        {
            _animator.SetBool(FALLING_PARAM, false);
        }

        public void BeginCrouch()
        {
            _animator.SetBool(CROUCH_PARAM, true);
        }

        public void EndCrouch()
        {
            _animator.SetBool(CROUCH_PARAM, false);
        }

        public void BeginClimb()
        {
            _animator.SetBool(CLIMBING_PARAM, true);
        }

        public void EndClimb()
        {
            _animator.SetBool(CLIMBING_PARAM, false);
        }

        public void BeginHang()
        {
            _animator.SetBool(HANGING_PARAM, true);
        }

        public void EndHang()
        {
            _animator.SetBool(HANGING_PARAM, false);
        }

        public void BeginLadder()
        {
            _animator.SetBool(LADDING_PARAM, true);
        }

        public void EndLadder()
        {
            _animator.SetBool(LADDING_PARAM, false);
        }

        public void BeginAttack()
        {
            _animator.SetBool(ATTACKING_PARAM, true);
        }

        public void EndAttack()
        {
            _animator.SetBool(ATTACKING_PARAM, false);
        }

        public void TriggerHurt()
        {
            _animator.SetTrigger(HURT_PARAM);
        }

        public void SetSpeed(float speed)
        {
            _animator.SetFloat(SPEED_PARAM, speed);
        }

        public void SetAttackCombo(int attackCombo)
        {
            _animator.SetInteger(ATTACKCOMBO_PARAM, attackCombo);
        }
    }
}