using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

[RequireComponent(typeof(UnitSelected))]
public class Unit : RTSObject
{
    public GameObject projectile;
    public int damage;

    public float fireRate;
    public float cooldown;

    public Transform[] turrets;
    public Transform turretEnd;

    [SerializeField]
    private bool canAttackAndRunAway;

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
        unitSelected = GetComponent<UnitSelected>();
    }

    //TODO: Fog of war is what dictates if a unit will follow after their enemy
    protected virtual void Update()
    {
        //Pathfinding
        if(unitSelected.selected)
        {
            // If we are not in range, become an agent again
            agent.enabled = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitAgentInfo, Mathf.Infinity))
            {
                //Layer 9 is enemy
                if (Input.GetMouseButtonDown(1) && !Mouse.IsDragging && hitAgentInfo.collider.gameObject.layer != 9)
                {
                    targetPosition = hitAgentInfo.point;
                    agent.destination = targetPosition;
                    agent.stoppingDistance = 0;
                }
            }
        }

        Fire();
    }

    protected virtual void LockOn()
    {
        AimTurrets();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if(Input.GetMouseButtonDown(1))
            {
                Debug.Log(hitInfo.collider.name);
                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nearestEnemy = hitInfo.transform.gameObject;

                    float sqrDistance = (nearestEnemy.transform.position - transform.position).sqrMagnitude;

                    if (sqrDistance > range)
                    {
                        agent.destination = nearestEnemy.transform.position;
                        agent.stoppingDistance = range;
                    }
                    else
                    {
                        agent.stoppingDistance = 0;
                    }
                }
                //If we tell the unit to "lock onto" the ground and it cant attack when running just make it run
                else if(!canAttackAndRunAway && hitInfo.collider.gameObject.name.Equals("RTSTerrain")) 
                {
                    nearestEnemy = null;
                }
                //Unit can run away and attack until its out of range
                else if(canAttackAndRunAway && hitInfo.collider.gameObject.name.Equals("RTSTerrain"))
                {
                    if ((nearestEnemy.transform.position - transform.position).sqrMagnitude > range * range)
                    {
                        nearestEnemy = null;
                    }
                }
            }
        }	
    }

    protected virtual void Fire()
    {
        if(unitSelected.selected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if(nearestEnemy != null)
            {
                cooldown -= Time.deltaTime;
                direction = nearestEnemy.transform.position - turretEnd.position;
                if (cooldown <= 0 && direction.sqrMagnitude <= range * range)
                {
                    var laser = Instantiate(projectile, turretEnd.transform.position, turretEnd.transform.rotation);
                    var hyperProjScript = laser.GetComponent<HyperbitProjectileScript>();
                    
                    cooldown = fireRate;
                    
                    hyperProjScript.owner = gameObject.name;
                    hyperProjScript.team = team;
                    hyperProjScript.damage = damage;
                    
                    int speed = hyperProjScript.speed;
                    
                    laser.GetComponent<Rigidbody>().AddForce(direction * speed);
                }
            }
            else
            {
                enemyHasBeenSelected = false;   
            }
        }
    }

    protected virtual void AimTurrets()
    {
        if (nearestEnemy != null)
        {
            Vector3 aimDirection = nearestEnemy.transform.position - transform.position;
            foreach (Transform turretTransform in turrets)
            {
                //Make each turret point towards the enemy target
                turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation, Quaternion.LookRotation(aimDirection), Time.time);
            }
        }
        
    }
}
