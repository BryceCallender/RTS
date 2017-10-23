using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : UnitStats
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
    private UnitSelected unitSelected;

    private bool enemyHasBeenSelected = false;

    private void Start()
    {
        turrentPosition = transform.Find("turret");
        originalTurretPosition = transform.rotation;
        unitSelected = GetComponent<UnitSelected>();
    }

    private void Update()
    {
        Fire();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void Fire()
    {
        if(unitSelected.selected || enemyHasBeenSelected)
        {
			LockOn();
            enemyHasBeenSelected = true;
            if(nearestEnemy != null)
            {
				fireCoolDown -= Time.deltaTime;
                direction = nearestEnemy.transform.position - this.transform.position;
				if (fireCoolDown <= 0 && direction.magnitude <= range)
				{
					fireCoolDown = 0.5f;
					GameObject projectile = (GameObject)Instantiate(bulletPrefab, turretEnd.transform.position, turretEnd.transform.rotation);
                    projectile.tag = "Laser";
                    //Ignores collisions between unit and bullet collision 
                    //once it initially fires and also ignores the collision with
                    //the ground
                    Physics.IgnoreLayerCollision(8,10);
                    Physics.IgnoreLayerCollision(0,8);
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

    public override void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }

    public override GameObject FindEnemy()
    {
        if(nearestEnemy == null)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            nearestEnemy = enemies[0] as GameObject;
        }
        else
        {
            
        }
        return nearestEnemy;
    }

    public void LockOn()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if(Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<UnitSelected>().selected)
            {
                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nearestEnemy = hitInfo.transform.gameObject;
                }
                else if(hitInfo.collider.gameObject.name == "RTSTerrain") 
                {
                    nearestEnemy = null;
                }
            }
        			
			if (direction.magnitude <= range)
			{
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    turrentPosition.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
                }
			}
            if(nearestEnemy == null || direction.magnitude > range)
			{
                turrentPosition.rotation = Quaternion.Lerp(Quaternion.Euler(direction), gameObject.GetComponent<Transform>().rotation, 1.0f);
			}
		}	
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Laser") && collision.gameObject.layer == 10)
        {
            TakeDamage(5);
        }
        else if (collision.gameObject.tag.Contains("Cluster") && collision.gameObject.layer == 10)
        {
            TakeDamage(10);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Physics.IgnoreLayerCollision(8, 10, false);
    }
}
