using System.Collections;
using System.Collections.Generic;
using TeamTheDream.Delivery;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.GetComponent<HealthManager>().TakeDamage(_enemy.Damage);
            player.HitParticle.Play();
            //Maybe we could include here a knockback
        }
    }
}
