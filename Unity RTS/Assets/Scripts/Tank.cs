using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unit;

[RequireComponent(typeof(BoxCollider))]
public class Tank : UnitScript, IUnitStats, IImageable
{
    public int capacity = 5;
    public Transform turretEnd;

    //Turrent rotations
    Transform turrentPosition;
    Quaternion originalTurretPosition;

    //Bullets and enemies
    public GameObject bulletPrefab;
    public GameObject nearestEnemy;
    private GameObject projectile;

    //Firing and dealing with unit
    private float fireCoolDown = 0.5f;
    private RaycastHit hitInfo;
    private Vector3 direction;
    private UnitSelected unitSelected;

    private float timerToStop = 0;
    private float timeToStopShowingHealth = 3.0f;
    private bool isUnderAttack;

    private bool enemyHasBeenSelected = false;
  
    private void Start()
    {
        
        projectile = bulletPrefab;
        
        turrentPosition = transform.Find("turret");
        originalTurretPosition = transform.rotation;
        unitSelected = GetComponent<UnitSelected>();
    }

    private void Update()
    {
        //if(unitSelected.selected)
        //{
        //    healthBar.gameObject.SetActive(true);
        //}

		if(unitSelected.isFirst)
		{
			ShowImage();
		}

        //if(!unitselected.selected && healthbar.gameobject.activeself)
        //{
        //    healthbarfadeaway();
        //}
    }

	private void FixedUpdate()
	{
		Fire();
	}

    public void Fire()
    {
        if(unitSelected.selected || enemyHasBeenSelected)
        {
			LockOn();
            enemyHasBeenSelected = true;
            if(nearestEnemy != null)
            {
				fireCoolDown -= Time.deltaTime;
                direction = nearestEnemy.transform.position - turretEnd.position;
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

	public void ShowImage()
	{
		UIManager.Instance.SetPhoto(this.gameObject.name);
	}
}
