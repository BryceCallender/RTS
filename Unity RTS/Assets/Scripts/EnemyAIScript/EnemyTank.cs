using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTank : Enemy
{
	public Transform turretEnd;
	Transform turrentPosition;
	Quaternion originalTurretPosition;

	public GameObject bulletPrefab;
	public List<GameObject> enemies;
	public GameObject nearestEnemy;

	private int randomObjectToAttack;
	private float fireCoolDown = 0.5f;
	private RaycastHit[] hitInfo;
	private Vector3 direction;

	GameObject projectile;
	private bool enemyHasBeenSelected = false;
	private Quaternion keepUIAbove;
	public Canvas canvas;

	

	// Use this for initialization
	void Start ()
	{
		keepUIAbove = canvas.GetComponent<RectTransform>().rotation;
		projectile = bulletPrefab;
		enemies = new List<GameObject>();
		originalTurretPosition = transform.rotation;
		turrentPosition = transform.Find("turret");
	}
	
	// Update is called once per frame
	void Update ()
	{
		canvas.GetComponent<RectTransform>().rotation = keepUIAbove;
	}

	private void FixedUpdate()
	{
		//Fire();
	}

	public void Fire()
	{
		//If AI has no enemy find one in its proximity if it even can 
		if (nearestEnemy == null)
		{
			LockOn();
			enemyHasBeenSelected = true;
		}

		if (enemyHasBeenSelected)
		{
			if (nearestEnemy != null)
			{
				fireCoolDown -= Time.deltaTime;
				direction = nearestEnemy.transform.position - this.transform.position;

				if (direction.magnitude <= range)
				{
					if (direction != Vector3.zero)
					{
						Quaternion lookRotation = Quaternion.LookRotation(direction);
						turrentPosition.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
					}
				}
				if (fireCoolDown <= 0 && direction.magnitude <= range)
				{
					fireCoolDown = 0.5f;
					projectile = (GameObject)Instantiate(bulletPrefab, turretEnd.transform.position, turretEnd.transform.rotation);
					projectile.tag = "Laser";
					projectile.GetComponent<HyperbitProjectileScript>().owner = gameObject.name;
					//projectile.transform.LookAt(nearestEnemy.transform.position);
					int speed = projectile.GetComponent<HyperbitProjectileScript>().speed;
					projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
					//projectile.GetComponent<HyperbitProjectileScript>().HitEnemy(nearestEnemy.transform.position);
				}
			}
			else
			{
				enemyHasBeenSelected = false;
			}
		}

		if (enemies.Count == 0)
		{
			turrentPosition.rotation = Quaternion.Lerp(Quaternion.Euler(direction), gameObject.GetComponent<Transform>().rotation, 1.0f);
		}
	}

	/*
     * Make it lock onto an enemy in its sphere of influence and it will randomize
     * which one it decides to pick to give it a sense of a stupid AI. 
     */
	public void LockOn()
	{
		enemies.Clear();
		hitInfo = Physics.SphereCastAll(transform.position, range, Vector3.forward);
		for (int i = 0; i < hitInfo.Length; i++)
		{
			if (hitInfo[i].collider.gameObject.layer == 8 || hitInfo[i].collider.gameObject.layer == 11)
			{
				enemies.Add(hitInfo[i].collider.gameObject);
			}
		}

		//Find enemy and then randomize enemy to lock on and then angle the 
		//turret to be aimed in order to hit the enemy.
		if (enemies.Count > 0)
		{
			randomObjectToAttack = (int)Random.Range(0, enemies.Count);
			nearestEnemy = enemies[randomObjectToAttack];
		}

	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
