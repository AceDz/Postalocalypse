using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public interface IAnimatorController
    {
        void BeginWalk();
        void EndWalk();
        void BeginRun();
        void EndRun();
        void BeginJump();
        void EndJump();
        void BeginFall();
        void EndFall();
        void BeginCrouch();
        void EndCrouch();
        void BeginClimb();
        void EndClimb();
        void BeginHang();
        void EndHang();
        void BeginLadder();
        void EndLadder();
        void BeginAttack();
        void EndAttack();
        void TriggerHurt();
        void SetAttackCombo(int attackChain);
        void SetSpeed(float speed);
    }
}