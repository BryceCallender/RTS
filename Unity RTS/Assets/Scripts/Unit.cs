using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitsAttackable
{
    Ground,
    Air,
    All
}

public enum UnitDamageStrength
{
    Ground,
    Air,
    None
}

public class Unit : RTSObject
{
    public GameObject projectile;
    public int damage;

    public float fireRate;
    public float cooldown;
    
    public Transform turretEnd;

    public UnitsAttackable unitsUnitCanAttack;
    public UnitDamageStrength unitsUnitIsStrongAgainst;
    
    public GameObject nearestEnemy;
    
    private RaycastHit hitInfo;
    private Vector3 direction;
    private UnitSelected unitSelected;

    private bool enemyHasBeenSelected = false;
    
    //Pathfinding variables
    public Vector3 targetPosition;
    private NavMeshAgent agent;
    private RaycastHit hitAgentInfo;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        //Pathfinding
        if(unitSelected.selected)
        {
            // If we are not in range, become an agent again
            agent.enabled = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitAgentInfo, Mathf.Infinity))
            {
                if (Input.GetMouseButtonDown(1) && !Mouse.IsDragging && hitAgentInfo.collider.gameObject.layer != 9)
                {
                    targetPosition = hitAgentInfo.point;
                    agent.destination = targetPosition;
                }
            }
        }
    }

    protected void LockOn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if(Input.GetMouseButtonDown(1))
            {
                Debug.Log(hitInfo.collider.name);
                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nearestEnemy = hitInfo.transform.gameObject;
                }
                else if(hitInfo.collider.gameObject.name == "RTSTerrain") 
                {
                    nearestEnemy = null;
                }
            }
        }	
    }

    protected void Fire()
    {
        if(unitSelected.selected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if(nearestEnemy != null)
            {
                cooldown -= Time.deltaTime;
                direction = nearestEnemy.transform.position - turretEnd.position;
                if (cooldown <= 0 && direction.magnitude <= range)
                {
                    var hyperProjScript = projectile.GetComponent<HyperbitProjectileScript>();
                    
                    cooldown = fireRate;
                    projectile = Instantiate(projectile, turretEnd.transform.position, turretEnd.transform.rotation);
                    hyperProjScript.owner = gameObject.name;
                    hyperProjScript.team = team;
                    int speed = hyperProjScript.speed;
                    
                    projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
                }
            }
            else
            {
                enemyHasBeenSelected = false;   
            }
        }
    }
}
