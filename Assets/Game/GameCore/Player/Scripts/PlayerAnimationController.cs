using System.Collections;
using System.Collections.Generic;
using TeamTheDream.Delivery;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private PlayerMovementController _player;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<PlayerMovementController>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed",Mathf.Abs(_rigidbody2D.velocity.x));
    }

    public IEnumerator LaunchAttackAnimation()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        _animator.SetTrigger("EndAttack");
    }

    public void LaunchGotHitAnimation()
    {
        _animator.SetTrigger("Hit");
    }

    public void LaunchDeadAnimation()
    {
        _animator.SetBool("IsDead", true);
    }

    public void Revive()
    {
        _animator.SetBool("IsDead", false);
    }
}
