using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : MechUnit
{
    protected override void Fire()
    {
        if (UnitIsSelected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null && DamageHelper.IsUnitAbleToAttack(gameObject, nearestEnemy))
            {
                cooldown -= Time.deltaTime;
                direction = (nearestEnemy.transform.position - turretEnd.position).normalized;
                direction.y += 0.05f; //Aim higher ???
                Debug.DrawRay(turretEnd.transform.position, direction * 10, Color.yellow, 1.0f);
                if (cooldown <= 0 && DoneAiming && direction.sqrMagnitude <= range * range)
                {

                }
            }
            else
            {
                enemyHasBeenSelected = false;
            }
        }
    }


    //This unit aims itself by just walking over to the unit
    protected override void AimTurrets() {}
}
