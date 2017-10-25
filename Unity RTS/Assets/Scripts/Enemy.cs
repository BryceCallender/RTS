﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitStats
{
    public int damage = 5;
    public int health = 100;
    public int range = 10;
    public int cost = 10;
    public Transform turretEnd;

    Transform turrentPosition;
    Quaternion originalTurretPosition;

    public GameObject bulletPrefab;
    public List<GameObject> enemies;
    public GameObject nearestEnemy;

    private int randomObjectToAttack;
    private float fireCoolDown = 0.5f;
   // private float fireCoolDownLeft = 0;
    private RaycastHit[] hitInfo;
    private Vector3 direction;

    GameObject projectile;
    private bool enemyHasBeenSelected = false;
    private int team = 1;

	// Use this for initialization
	void Start() 
    {
        projectile = bulletPrefab;
        enemies = new List<GameObject>();
        originalTurretPosition = transform.rotation;
        turrentPosition = transform.Find("turret");
	}
	
	// Update is called once per frame
	void Update () 
    {
        //LockOn();
        Fire();
	}

    public override void Die()
	{
		Destroy(gameObject);
	}


	public override void TakeDamage(int damage)
	{
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }

	}

    public override void Fire()
    {
        //If AI has no enemy find one in its proximity if it even can 
        if(nearestEnemy == null)
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
                    projectile.GetComponent<HyperbitProjectileScript>().team = team;
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
    }

    public override GameObject FindEnemy()
    {
        if (nearestEnemy == null)
        {
            
        }
        else
        {

        }
        return nearestEnemy;
    }

    /*
     * Make it lock onto an enemy in its sphere of influence and it will randomize
     * which one it decides to pick to give it a sense of a stupid AI. 
     */
    public void LockOn()
    {
        hitInfo = Physics.SphereCastAll(this.transform.position, range, Vector3.forward);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if(hitInfo[i].collider.gameObject.layer == 8 || hitInfo[i].collider.gameObject.layer == 11)
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

    //When something enters the collider take damage to the unit!
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(9, 10);
        }

        if (!collision.gameObject.GetComponent<HyperbitProjectileScript>().owner.Equals(gameObject.name)
            && !collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(9, 10, false);
            if (collision.gameObject.tag.Contains("Laser") && collision.gameObject.layer == 10)
            {
                TakeDamage(5);
            }
            else if (collision.gameObject.tag.Contains("Cluster") && collision.gameObject.layer == 10)
            {
                TakeDamage(10);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
