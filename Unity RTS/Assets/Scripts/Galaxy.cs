using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Galaxy : UnitStats 
{
	public int damage = 10;
	public int health = 150;
	public int range = 10;
    public int cost = 20;

	public GameObject[] enemies;
	public GameObject nearestEnemy;
    public GameObject Thruster;
    public GameObject bulletPrefab;
    [SerializeField]
    private GameObject[] turrets;


	private float fireCoolDown = 1.5f;
	private float fireCoolDownLeft = 0;
	private RaycastHit hitInfo;
	private Vector3 direction;
    private ParticleSystem thruster;
    private Vector3 position;
    private NavMeshAgent agent;

    GameObject projectile;
    private bool enemyHasBeenSelected = false;

    private void Start()
    {
        projectile = bulletPrefab;
        thruster = Thruster.GetComponentInChildren<ParticleSystem>();
        turrets = GameObject.FindGameObjectsWithTag("GalaxyTurrets");

        //for (int i = 0; i < turrets.Length; i++)
        //{
        //    turrets[i].transform.LookAt(nearestEnemy.transform.position);
        //}
        thruster.Stop();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Fire();
        if(agent.velocity != Vector3.zero)
        {
            ActivateThrusters();
        }
        else
        {
            DeActivateThrusters();
        }
    }

	public override GameObject FindEnemy()
	{
		if (nearestEnemy == null)
		{
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			nearestEnemy = enemies[0] as GameObject;
		}
		else
		{

		}
		return nearestEnemy;
	}

    public override void Fire()
    {
        if (this.gameObject.GetComponent<UnitSelected>().selected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null)
            {
                GameObject turretToFire = turrets[RandomizeTurretSelection()];
                direction = nearestEnemy.transform.position - turretToFire.transform.position;
                fireCoolDownLeft -= Time.deltaTime;
                if (fireCoolDownLeft <= 0 && direction.magnitude <= range)
                {
                    fireCoolDownLeft = fireCoolDown;
                    projectile = (GameObject)Instantiate(bulletPrefab, turretToFire.transform.position, turretToFire.transform.rotation);
                    projectile.tag = "Cluster";
                    projectile.GetComponent<HyperbitProjectileScript>().owner = gameObject.name;

                    //Physics.IgnoreLayerCollision(8, 10);
                    //Physics.IgnoreLayerCollision(0, 10);
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

	public void LockOn()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
		{
			if (Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<UnitSelected>().selected)
			{
				if (hitInfo.collider.gameObject.CompareTag("Enemy"))
				{
					nearestEnemy = hitInfo.transform.gameObject;
				}
				else if (hitInfo.collider.gameObject.name == "RTSTerrain")
				{
					nearestEnemy = null;
				}
                if(nearestEnemy != null)
                {
					for (int i = 0; i < turrets.Length; i++)
					{
						turrets[i].transform.LookAt(nearestEnemy.transform.position);
					}
                }
			}

			if (direction.magnitude <= range)
			{
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                }
				//turrentPosition.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
			}
            if (nearestEnemy == null || direction.magnitude > range)
            {
                //foreach(GameObject turrentRotation in turrets)
                //{
                //    turrentRotation.transform.rotation = Quaternion.Lerp(Quaternion.Euler(direction), gameObject.GetComponent<Transform>().rotation, 1.0f);
                //}
            }
		}
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

    public void ActivateThrusters()
    {
        thruster.Play();
    }

	public void DeActivateThrusters()
	{
        thruster.Stop();
	}

    public int RandomizeTurretSelection()
    {
        int random;
        random = (int)Random.Range(0,turrets.Length);
        return random;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<HyperbitProjectileScript>().owner.Equals(gameObject.name))
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

    private void OnCollisionExit(Collision collision)
    {
        Physics.IgnoreLayerCollision(8, 10, false);
    }
}
