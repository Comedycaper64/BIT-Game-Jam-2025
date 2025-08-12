using System;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private int maxHealth = 1;
    private int health;

    public Action OnTakeDamage;
    public Action OnDeath;

    private void Start()
    {
        SetMaxHealth();
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public void SetMaxHealth()
    {
        health = maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        if (health == 0f)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }
}
