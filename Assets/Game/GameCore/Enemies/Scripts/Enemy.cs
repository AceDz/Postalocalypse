using System;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using I2.Loc;
using MEC;
using UnityEngine.Serialization;
using System.Collections;

namespace TeamTheDream.Delivery
{
    public class Enemy : SerializedWorldActor
    {
        [SerializeField, BoxGroup("Behaviour")] private float _attackCooldown;
        [SerializeField, BoxGroup("Behaviour")] private int _damage;
        [SerializeField, BoxGroup("Behaviour")]private int _health;
        [FormerlySerializedAs("_sightRange")] [SerializeField, BoxGroup("Behaviour")]  private float _meleeRange;
        [FormerlySerializedAs("_colliderDistance")] [SerializeField, BoxGroup("Behaviour")] private float _meleeColliderDistance;
        [SerializeField, BoxGroup("Physics")] private CapsuleCollider2D _capsuleCollider;
        [SerializeField, BoxGroup("Physics")] private LayerMask _playerLayer;
        [SerializeField, BoxGroup("Audio")] private EnemyAudio _audio;

        private bool _isAlive;
        private bool _isHurtAnimating;
        private bool _isAttacking;
        private float _cooldownTimer = Mathf.Infinity;
        private Animator _animator;

        public int Damage => _damage;
        public bool IsAlive => _isAlive;
        public bool IsHurting => _isHurtAnimating;
        public bool IsAttacking => _isAttacking;

        private void Awake()
        {
            _isAlive = true;
            _isHurtAnimating = false;
            _isAttacking = false;
            _animator = GetComponentInChildren<Animator>();
        }

        protected void Update()
        {
            _cooldownTimer += Time.deltaTime;

            //Attack only when player in sight
            if (PlayerInSight())
            {
                if (_cooldownTimer >= _attackCooldown)
                {
                    _cooldownTimer = 0;
                    StartCoroutine(AttackAnimation());
                }
            }
        }

        private IEnumerator AttackAnimation()
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
            _audio.PlayAttack();
            yield return new WaitForSeconds(0.8f);
            if (PlayerInSight())
            {
                var player = FindObjectOfType<PlayerMovementController>();
                if (player != null)
                {
                    player.GetComponent<HealthManager>().TakeDamage(Damage);
                    player.HitParticle.Play();
                }
                    
            }
            _isAttacking = false;
            Debug.Log("Test Damage");
        }

        private bool PlayerInSight()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                _capsuleCollider.bounds.center + transform.right * _meleeRange * transform.localScale.x * _meleeColliderDistance,
                new Vector3(_capsuleCollider.bounds.size.x * _meleeRange, _capsuleCollider.bounds.size.y, _capsuleCollider.bounds.size.z),
                0,
                Vector2.left,
                0,
                _playerLayer);
            
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            if (_capsuleCollider == null)
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                _capsuleCollider.bounds.center + transform.right * _meleeRange * transform.localScale.x * _meleeColliderDistance, 
                new Vector3(_capsuleCollider.bounds.size.x * _meleeRange, _capsuleCollider.bounds.size.y, _capsuleCollider.bounds.size.z));
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            Debug.Log($"Health remaining: {_health}");

            if (_health <= 0)
            {
                Die();
                return;
            }

            _audio.PlayHurt();
            StartCoroutine(HurtAnimation());
            
        }

        private IEnumerator HurtAnimation()
        {
            _isHurtAnimating = true;
            _animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.5f);
            _isHurtAnimating = false;
        }

        public void Die()
        {
            Debug.Log("Enemy defeated!");
            _isAlive = false;
            _animator.SetTrigger("Dead");
            _audio.PlayDead();
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject,3);
        }
    }
}

