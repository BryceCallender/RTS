using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AntiAir))]
public class AATurret : Turret
{
    AntiAir antiAir;

    private MissileInfo missileInfo;

    protected override void Start()
    {
        base.Start();

        antiAir = GetComponent<AntiAir>();
        antiAir.SetMissileReloadTime(fireRate);
    }

    protected override void Fire()
    {
        LockOn();
        cooldown -= Time.deltaTime;

        if (targetedEnemy != null)
        {    
            if (cooldown <= 0 && direction.sqrMagnitude <= range * range)
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

                    homingMissile.enemy = targetedEnemy;
                }
            }
        }
    }

    protected override void LockOn()
    {
        missileInfo = antiAir.FindAvailableMissileToLaunch();

        //None are available so dont go further
        if (missileInfo.missileIndex == -1)
            return;

        //We arew guarenteed a transform since the missile index was not -1 which is set when none are avial.
        turretEnd = missileInfo.missileTransform;

        base.LockOn();
    }
}
