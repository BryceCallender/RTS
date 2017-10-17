using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int health;

	// Use this for initialization
	void Start () 
    {
        health = 20;
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

	public void Die()
	{
		Destroy(gameObject);
	}

	public void TakeDamage(int damage)
	{
		if (health > 0)
		{
			health -= damage;
		}
		else
		{
			Die();
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
