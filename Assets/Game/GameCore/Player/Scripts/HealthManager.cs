using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TeamTheDream;
using TeamTheDream.Delivery;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Health")] private int _maxHealth = 3;
    [SerializeField, BoxGroup("Health")] private float _invulnerabilityTime = 0.5f;
    
    [SerializeField, BoxGroup("Items")] private LifeItem _lifeItem;
    [SerializeField, BoxGroup("Items")] private SuperLifeItem _superLifeItem;
    [SerializeField, BoxGroup("Items")] private ExtraLifeItem _extraLifeItem;
    
    [SerializeField, BoxGroup("Audio")] 
    private AudioManager _audio;

    public static Action<int> OnHealthChange;

    public static System.Action OnPlayerDeath; 

    private int _currentHealth;
    private float _invulnerabilityTimeRemaining = 0;

    public int Health => _currentHealth;
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        _lifeItem.OnAmountChanged += RegenerateLife;
        _superLifeItem.OnAmountChanged += FullHealth;
        _extraLifeItem.OnAmountChanged += AddExtraLife;
    }

    private void OnDisable()
    {
        _lifeItem.OnAmountChanged -= RegenerateLife;
        _superLifeItem.OnAmountChanged -= FullHealth;
        _extraLifeItem.OnAmountChanged -= AddExtraLife;
    }

    private void FullHealth(int amount)
    {
        _currentHealth = _maxHealth;
        OnHealthChange?.Invoke(Health);
    }
    
    public void Revive()
    {
        _currentHealth = _maxHealth;
        OnHealthChange?.Invoke(Health);
        GetComponent<PlayerAnimationController>().Revive();
    }

    private void Update()
    {
        if (_invulnerabilityTimeRemaining > 0)
            _invulnerabilityTimeRemaining -= Time.deltaTime;
    }
    public void TakeDamage (int damage)
    {
        if (enabled && _invulnerabilityTimeRemaining <= 0)
        {
            _audio.PlayProjectileHitsSfx();
            _invulnerabilityTimeRemaining = _invulnerabilityTime;
            _currentHealth -= damage;
            Debug.Log($"Health remaining: {_currentHealth}");

            if (_currentHealth <= 0)
                Die();
            else
                GetComponent<PlayerAnimationController>().LaunchGotHitAnimation();

            OnHealthChange?.Invoke(Health);
        }
    }

    public void RegenerateLife(int amount)
    {
        _currentHealth += amount;
        
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        OnHealthChange?.Invoke(Health);
    }

    public void AddExtraLife(int amount)
    {
        _maxHealth++;
        RegenerateLife(1);
    }

    public void Die()
    {
        _audio.PlayGameOverMusic();
        GetComponent<PlayerMovementController>().Die();
        GetComponent<PlayerAnimationController>().LaunchDeadAnimation();
        OnPlayerDeath?.Invoke();
    }
}
