﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitsAttackable
{
    Ground,
    Air,
    All
}

[RequireComponent(typeof(UnitSelected))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AttackInfo))]
public class Unit : RTSObject, ISelectable
{
    public GameObject projectile;
    public int damage;
    public int loadSize;

    [SerializeField]
    protected float turnSpeed = 5;

    public float fireRate;
    [HideInInspector]
    public float cooldown;

    public Transform[] turrets;
    public Transform turretEnd;

    [SerializeField]
    private readonly bool canAttackAndRunAway;

    private AttackInfo attackInfo;
    
    //[HideInInspector]
    public GameObject nearestEnemy;
    
    protected RaycastHit hitInfo;
    protected Vector3 direction;
    protected UnitSelected unitSelected;

    protected bool UnitIsSelected => unitSelected.selected;
    protected bool enemyHasBeenSelected;
    
    //Pathfinding variables
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public NavMeshAgent agent;
    private RaycastHit hitAgentInfo;

    private float lerpTime;
    public bool DoneAiming => lerpTime > 1.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        unitSelected = GetComponent<UnitSelected>();
        attackInfo = GetComponent<AttackInfo>();
    }

    protected virtual void Start() { }

    //TODO: Fog of war is what dictates if a unit will follow after their enemy
    //TODO: If the enemy is locked on and is leaving once out of range reset the turret!
    protected virtual void Update()
    {
        //Pathfinding
        if(UnitIsSelected)
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
            if(UnitIsSelected && Input.GetMouseButtonDown(1))
            {
                Debug.Log(hitInfo.collider.name);
                if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nearestEnemy = hitInfo.transform.gameObject;

                    float sqrDistance = (nearestEnemy.transform.position - transform.position).sqrMagnitude;

                    if (sqrDistance > range * range)
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
                    ResetTurrets();
                }
                //Unit can run away and attack until its out of range
                else if(canAttackAndRunAway && hitInfo.collider.gameObject.name.Equals("RTSTerrain"))
                {
                    if ((nearestEnemy.transform.position - transform.position).sqrMagnitude > range * range)
                    {
                        nearestEnemy = null;
                        ResetTurrets();
                    }
                }
            }
        }	
    }

    protected virtual void Fire()
    {
        if(UnitIsSelected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if(nearestEnemy != null && DamageHelper.IsUnitAbleToAttack(gameObject, nearestEnemy))
            {
                cooldown -= Time.deltaTime;
                direction = (nearestEnemy.transform.position - turretEnd.position).normalized;
                direction.y += 0.05f; //Aim higher ???
                Debug.DrawRay(turretEnd.transform.position, direction * 10, Color.yellow, 1.0f);
                if (cooldown <= 0 && DoneAiming && direction.sqrMagnitude <= range * range)
                {
                    if (projectile != null)
                    {
                        var laser = Instantiate(projectile, turretEnd.transform.position, turretEnd.transform.rotation);
                        var hyperProjScript = laser.GetComponent<HyperbitProjectileScript>();
                    
                        cooldown = fireRate;
                    
                        hyperProjScript.owner = gameObject.name;
                        hyperProjScript.team = team;
                        hyperProjScript.damage = damage;
                        hyperProjScript.unitDamageStrength = armorClass;

                        //laser.GetComponent<Rigidbody>().velocity = direction * hyperProjScript.speed;
                        laser.GetComponent<Rigidbody>().AddForce(direction * hyperProjScript.speed);
                    }   
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
        if(nearestEnemy != null)
        {
            Vector3 aimDirection = nearestEnemy.transform.position - transform.position;
            lerpTime += Time.deltaTime * turnSpeed;
            foreach (Transform turretTransform in turrets)
            {
                //Make each turret point towards the enemy target
                turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation, Quaternion.LookRotation(aimDirection), lerpTime);
            }
        }
        else
        {
            ResetTurrets();
        }
    }

    protected virtual void ResetTurrets()
    {
        lerpTime = 0f;
        foreach (Transform turretTransform in turrets)
        {
            //Make each turret point towards the enemy target
            turretTransform.localRotation = Quaternion.identity;
            //turretTransform.localRotation = Quaternion.Lerp(turretTransform.rotation, Quaternion.identity, Time.deltaTime * turnSpeed);
        }
    }
}
