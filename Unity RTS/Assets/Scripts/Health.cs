using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public bool isRepairable; //TODO::figure out if I want to do an IRepairable which defines how to repair

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //TODO::Implement a way to repair
    public virtual void Repair(float repairAmount)
    {
        if(isRepairable)
        {
            currentHealth += repairAmount;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void ChangeMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }

}
