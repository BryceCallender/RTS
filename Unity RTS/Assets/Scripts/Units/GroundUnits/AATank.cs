using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AntiAir))]
public class AATank : Unit
{
    AntiAir antiAir;

    protected override void Start()
    {
        base.Start();

        antiAir = GetComponent<AntiAir>();
        antiAir.SetMissileReloadTime(fireRate);
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
                MissileInfo missileInfo = antiAir.FindAvailableMissileToLaunch();

                //None are available so dont go further
                if (missileInfo.missileIndex == -1)
                    return;

                //We arew guarenteed a transform since the missile index was not -1 which is set when none are avial.
                turretEnd = missileInfo.missileTransform;

                direction = (nearestEnemy.transform.position - turretEnd.position).normalized;

                if (cooldown <= 0 && DoneAiming && direction.sqrMagnitude <= range * range)
                {
                    if (projectile != null)
                    {
                        antiAir.EnableMissile(missileInfo.missileIndex, false);
                        StartCoroutine(antiAir.ReloadMissileHead(missileInfo.missileIndex));

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
