using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    private int maxHealth;
    private int currentHealth;
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        //TODO: Implement Die method
    }
}
