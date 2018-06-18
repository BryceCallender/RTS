using UnityEngine;
using UnityEngine.UI;
using Unit;

public class Enemy : UnitScript
{
    public int range = 10;

	public Slider healthBar;
	private bool hasCollided = false;

	private void Start()
	{
		healthBar.gameObject.SetActive(false);
		healthBar.maxValue = health;
		healthBar.value = health;
	}
}
