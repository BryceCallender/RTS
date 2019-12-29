using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackInfo))]
public class Turret : Building
{
    [SerializeField]
    protected List<GameObject> targets = new List<GameObject>();

    public GameObject projectile;
    public Transform turretPivot;
    public Transform turretEnd;
    public float turnSpeed;
    public int damage;

    public float fireRate;
    [HideInInspector]
    public float cooldown;

    private AttackInfo attackInfo;

    public float idleWaitTime = 2.0f;
    private float currentIdleTime = 0.0f;

    private float rotationDirection;

    /// <summary>
    /// The time it takes for the tower to correct its x rotation on idle in seconds
    /// </summary>
    public float idleCorrectionTime = 2.0f;

    /// <summary>
    /// Counter used for x rotation correction
    /// </summary>
    protected float xRotationCorrectionTime;

    private bool directionPicked = false;
    private Vector3 direction;
    [SerializeField]
    private GameObject targetedEnemy;

    protected override void Start()
    {
        base.Start();

        attackInfo = GetComponent<AttackInfo>();
    }

    protected override void Update()
    {
        base.Update();

        SearchForEnemies();

        //If no enemies have been found after the search
        if(targets.Count == 0)
        {
            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleWaitTime)
            {
                if (!directionPicked)
                {
                    rotationDirection = Random.value * 2 - 1;
                    directionPicked = true;
                }
                RotateTurretPivot();
            }
        }
        else
        {
            currentIdleTime = 0.0f;
            directionPicked = false;

            if (targetedEnemy == null)
            {
                targetedEnemy = targets[0]; //First enemy to have been spotted
            }
            else
            {
                Fire();
            }
        }
    }

    private void RotateTurretPivot()
    {
        Vector3 euler = turretPivot.rotation.eulerAngles;
        euler.x = Mathf.Lerp(Wrap180(euler.x), 0, xRotationCorrectionTime);
        xRotationCorrectionTime = Mathf.Clamp01((xRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
        euler.y += rotationDirection * turnSpeed * Time.deltaTime;

        turretPivot.eulerAngles = euler;
    }

    /// <summary>
	/// A simple function to convert an angle to a -180/180 wrap
	/// </summary>
	public static float Wrap180(float angle)
    {
        angle %= 360;
        if (angle < -180)
        {
            angle += 360;
        }
        else if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

    private void SearchForEnemies()
    {
        targets.Clear();
        Collider[] hitInfo;
        int layerMask;

        if(team == Team.Blue)
        {
            layerMask = 1 << 9; //Only collide with enemy units
        }
        else
        {
            layerMask = 1 << 8; //Only collide with player units
        }

        hitInfo = Physics.OverlapSphere(transform.position, range, layerMask);

        for (int i = 0; i < hitInfo.Length; i++)
        {
            targets.Add(hitInfo[i].gameObject);
        }
    }

    protected void Fire()
    {
        LockOn();
        cooldown -= Time.deltaTime;

        if (targetedEnemy != null && DamageHelper.IsUnitAbleToAttack(gameObject, targetedEnemy))
        {
            Debug.DrawRay(turretEnd.transform.position, direction * 10, Color.yellow, 1.0f);
            if (cooldown <= 0 && direction.sqrMagnitude <= range * range)
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
    }

    private void LockOn()
    {
        direction = (targetedEnemy.transform.position - turretEnd.position).normalized;
        direction.y += 0.05f; //Aim higher ???

        turretPivot.rotation = Quaternion.LookRotation(direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.up * 5);
    }
}
