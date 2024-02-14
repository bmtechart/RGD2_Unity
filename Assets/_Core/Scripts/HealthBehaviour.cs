using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour, IDamageable
{

    private bool _isDead = false;

    /// <summary>
    /// Health expressed as a value between 0-1
    /// </summary>
    public float NormalizedHealth {  get; private set; }

    [SerializeField]
    private float _maxHealth = 1.0f;

    /// <summary>
    /// Total pool of health points.
    /// </summary>
    public float MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
            NormalizedHealth = (float)_health / _maxHealth;
            HealthChanged?.Invoke(NormalizedHealth);
        }
    }

    [SerializeField]
    private float _health = 1.0f;
    /// <summary>
    /// Current health points. 
    /// </summary>
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            NormalizedHealth = (float)_health / _maxHealth;
            HealthChanged?.Invoke(NormalizedHealth);
            
            //check for death if not already dead
            if(!_isDead)
            {
                if (_health <= 0.0f) Die();
            }
            
        }
    }

    public UnityEvent<float> HealthChanged;
    public UnityEvent OnDeath;

    public void Damage(float amount)
    {
        Health -= amount;
    }

    public void Heal(float amount)
    {
        Health += amount;
    }

    private void Die()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }
}
