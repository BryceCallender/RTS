using System.Collections;
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
    public GameObject[] enemies;
    public GameObject nearestEnemy;

    private float fireCoolDown = 0.5f;
    private float fireCoolDownLeft = 0;
    private RaycastHit hitInfo;
    private Vector3 direction;
    private Vector3 bulletDirection;
    private float distance;

    private bool enemyHasBeenSelected = false;

	// Use this for initialization
	void Start() 
    {
        originalTurretPosition = transform.rotation;
        turrentPosition = transform.Find("turret");
	}
	
	// Update is called once per frame
	void Update () 
    {
        LockOn();
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
        if (enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null)
            {
                fireCoolDown -= Time.deltaTime;
                direction = nearestEnemy.transform.position - this.transform.position;
                if (fireCoolDown <= 0 && direction.magnitude <= range)
                {
                    fireCoolDown = 0.5f;
                    GameObject projectile = (GameObject)Instantiate(bulletPrefab, turretEnd.transform.position, turretEnd.transform.rotation);
                    projectile.tag = "Laser";
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

    public void LockOn()
    {
        if(Physics.SphereCast(this.transform.position,range,Vector3.forward, out hitInfo))
        {
            if(hitInfo.collider.gameObject.layer == 8)
            {
                nearestEnemy = hitInfo.collider.gameObject;
            }
        }
    }

    //When something enters the collider take damage to the unit!
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Contains("Laser"))
        {
            TakeDamage(5);
        }
        else if(collision.gameObject.tag.Contains("Cluster"))
        {
            TakeDamage(10);
        }
    }
}
