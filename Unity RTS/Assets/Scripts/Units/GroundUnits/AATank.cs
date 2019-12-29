using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MissileLauncher
{
    public GameObject missileHead;
    public bool isAvailable;
}

public class AATank : Unit
{
    [HideInInspector]
    public float missileReloadTime;
    private const int MISSILE_COUNT = 3;

    public MissileLauncher[] missiles = new MissileLauncher[MISSILE_COUNT];

    protected override void Start()
    {
        base.Start();
        missileReloadTime = fireRate + 0.5f;
    }

    protected int FindAvailableMissileToLaunch()
    {
        bool available = false;
        int missileIndex = 0;

        if (!AreMissilesAvailable())
            return -1;

        //Loop until a missile is available
        while (!available)
        {
            missileIndex = Random.Range(0, MISSILE_COUNT);
            available = missiles[missileIndex].isAvailable;
        }

        turretEnd = missiles[missileIndex].missileHead.transform;
        return missileIndex;
    }

    protected override void Fire()
    {
        if (UnitIsSelected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null && DamageHelper.IsUnitAbleToAttack(gameObject, nearestEnemy))
            {
                cooldown -= Time.deltaTime;
                int missileIndex = FindAvailableMissileToLaunch();

                //None are available so dont go further
                if (missileIndex == -1)
                    return;

                direction = (nearestEnemy.transform.position - turretEnd.position).normalized;

                if (cooldown <= 0 && DoneAiming && direction.sqrMagnitude <= range * range)
                {
                    if (projectile != null)
                    {
                        EnableMissile(missileIndex, false);
                        StartCoroutine(ReloadMissileHead(missileIndex));

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

    private bool AreMissilesAvailable()
    {
        int unavailableMissiles = 0; 

        for(int i = 0; i < MISSILE_COUNT; i++)
        {
            if(!missiles[i].isAvailable)
            {
                unavailableMissiles++;
            }
        }

        //Missiles are available if the number of unavilable are less than missile count
        return unavailableMissiles < MISSILE_COUNT;
    }

    private void EnableMissile(int missileIndex, bool enabled)
    {
        //Make the missile be shot!
        missiles[missileIndex].isAvailable = enabled;
        missiles[missileIndex].missileHead.SetActive(enabled);
    }

    private IEnumerator ReloadMissileHead(int missileIndex)
    {
        yield return new WaitForSeconds(missileReloadTime);
        EnableMissile(missileIndex, true);
    }
}