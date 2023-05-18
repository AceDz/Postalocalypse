using System.Collections;
using System.Collections.Generic;
using TeamTheDream.Delivery;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerMovementController>();
        if (player == null)
            return;

        var healthManager = player.GetComponent<HealthManager>();
        healthManager.TakeDamage(healthManager.MaxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovementController>();
        if (player == null)
            return;

        var healthManager = player.GetComponent<HealthManager>();
        healthManager.TakeDamage(healthManager.MaxHealth);
    }
}