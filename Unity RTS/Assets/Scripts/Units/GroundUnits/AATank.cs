using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AATank : Unit
{
    public float missileReloadTime;

    private const int MISSILE_COUNT = 3;
    public bool[] missileAvailability = new bool[MISSILE_COUNT];

    protected override void Start()
    {
        base.Start();
        missileAvailability.Populate(true);
    }

    protected int FindAvailableMissileToLaunch()
    {
        bool available = false;
        int missileIndex = 0;

        if (missileAvailability.AllMatchValue(false))
            return -1;

        //Loop until a missile is available
        while (!available)
        {
            missileIndex = Random.Range(0, turrets.Length);
            available = missileAvailability[missileIndex];
        }

        turretEnd = turrets[missileIndex];
        return missileIndex;
    }

    protected override void Fire()
    {
        if (UnitIsSelected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null && IsUnitAbleToAttack(nearestEnemy))
            {
                cooldown -= Time.deltaTime;
                int missileIndex = FindAvailableMissileToLaunch();

                //None are available so dont go further
                if (missileIndex == -1)
                    return;

                direction = (nearestEnemy.transform.position - turretEnd.position).normalized;

                if (cooldown <= 0 && direction.sqrMagnitude <= range * range)
                {
                    if (projectile != null)
                    {
                        //Make the missile be shot!
                        missileAvailability[missileIndex] = false;

                        var laser = Instantiate(projectile, turretEnd.transform.position, turretEnd.transform.rotation);
                        var hyperProjScript = laser.GetComponent<HyperbitProjectileScript>();
                        var homingMissile = laser.GetComponent<HomingMissile>();

                        cooldown = fireRate;

                        hyperProjScript.owner = gameObject.name;
                        hyperProjScript.team = team;
                        hyperProjScript.damage = damage;
                        hyperProjScript.unitDamageStrength = armorClass;

                        homingMissile.enemy = nearestEnemy;
                    }
                }
            }
            else
            {
                enemyHasBeenSelected = false;
            }
        }
    }
}