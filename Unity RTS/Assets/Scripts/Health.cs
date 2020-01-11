using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public bool isRepairable; //TODO::figure out if I want to do an IRepairable which defines how to repair

    private bool hasTakenDamage;
    private float timeToRestartRegens = 2.0f;
    private float restartRegenSystemsTimer = 0f;

    private void Update()
    {
        if(hasTakenDamage)
        {
            restartRegenSystemsTimer += Time.deltaTime;

            if(restartRegenSystemsTimer >= timeToRestartRegens)
            {
                restartRegenSystemsTimer = 0f;
                hasTakenDamage = false;
            }
        }
        
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hasTakenDamage = true;

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

    public Color HealthToColor()
    {
        float percentage = currentHealth / maxHealth;
        if(percentage >= 0.90)
        {
            return Color.green;
        }
        else if(percentage < 0.90 && percentage > 0.10)
        {
            return Color.yellow;
        }
        else
        {
            return Color.red;
        }
    }

}
