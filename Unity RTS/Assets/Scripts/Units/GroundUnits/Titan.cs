using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnergySystem))]
public class Titan : MechUnit
{
    public GameObject shield;
    private EnergySystem shieldSystem;

    protected override void Start()
    {
        base.Start();
        shieldSystem = GetComponent<EnergySystem>();
    }

    protected void Update()
    {
        base.Update();

        shield.SetActive(shieldSystem.HasEnergy);
    }


    //Need this function here to get rid of the specifics of the unit fire since it involves 
    //having a turret with a turretEnd transform however the Titan uses its FIST
    protected override void Fire()
    {
        if (UnitIsSelected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null && DamageHelper.IsUnitAbleToAttack(gameObject, nearestEnemy))
            {
                direction = (nearestEnemy.transform.position - transform.position).normalized;
                direction.y += 0.05f; //Aim higher ???
                //Do nothing rooVV cuz animation is going to be punching
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
