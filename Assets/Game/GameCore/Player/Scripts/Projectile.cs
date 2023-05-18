    using System.Collections;
using System.Collections.Generic;
using TeamTheDream;
using TeamTheDream.Delivery;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private int _damage;
    private AudioManager _audioManager;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void Setup(Vector2 force, int damage, AudioManager audioManager)
    {
        _damage = damage;
        _audioManager = audioManager;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        _audioManager.PlayShotSfx(force.sqrMagnitude); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile collides with " + collision.name);
        
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            _audioManager.PlayProjectileHitsSfx();    
            enemy.TakeDamage(_damage);
        }
        else
        {
         _audioManager.PlayProjectileHitsSfx();   
        }
        _particleSystem.Play();

        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.isKinematic = true;
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
        Destroy(this.gameObject,2);
    }
}
