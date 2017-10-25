using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private RaycastHit hitInfo;
    private Vector3 direction;
    private UnitSelected unitSelected;
    private int team = 0;

    GameObject projectile;
    private bool enemyHasBeenSelected = false;
    public Slider healthBar;


    private void Start()
    {
        healthBar.gameObject.SetActive(false);
        projectile = bulletPrefab;
        healthBar.maxValue = health;
        healthBar.value = health;
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
					projectile = (GameObject)Instantiate(bulletPrefab, turretEnd.transform.position, turretEnd.transform.rotation);
                    projectile.tag = "Laser";
                    projectile.GetComponent<HyperbitProjectileScript>().owner = gameObject.name;
                    projectile.GetComponent<HyperbitProjectileScript>().team = team;
					//projectile.transform.LookAt(nearestEnemy.transform.position);
					int speed = projectile.GetComponent<HyperbitProjectileScript>().speed;
					projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
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
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.value -= damage;
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
        if(collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(8,10);
            //Debug.Log("Same team bro");
        }

        if (!collision.gameObject.GetComponent<HyperbitProjectileScript>().owner.Equals(gameObject.name)
            && !collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
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
    }
}
