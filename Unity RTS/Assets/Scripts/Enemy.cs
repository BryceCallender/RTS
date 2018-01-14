using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damage = 5;
	public float health = 100;
    public int range = 10;
    public int cost = 10;

	public Slider healthBar;
	public HyperbitProjectileScript hyperProjectileScript;
	public int team = 1;

	private bool hasCollided= false;

	private void Start()
	{
		healthBar.gameObject.SetActive(false);
		healthBar.maxValue = health;
		healthBar.value = health;
	}

	public void Die()
	{
		Destroy(gameObject);
	}

	public void TakeDamage(float damage)
	{
		healthBar.gameObject.SetActive(true);
		health -= damage;
		healthBar.value -= damage;
		Debug.Log(health);
		if (health <= 0)
		{
			Die();
		}
	}
}
