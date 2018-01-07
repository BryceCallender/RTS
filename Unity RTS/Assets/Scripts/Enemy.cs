using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 5;
    public float health = 100;
    public int range = 10;
    public int cost = 10;

    public void Die()
	{
		Destroy(gameObject);
	}

	public void TakeDamage(float damage)
	{
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
	}
}
