using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public float abilityDuration;
    public float energyConsumption;

    private EnergySystem energySystem; //The objects energy system that it is attached to

    private void Update()
    {
        if(abilityDuration <= 0)
        {
            Destroy(this);
        }

        abilityDuration -= Time.deltaTime;
    }

    //Perform the action
    protected virtual void OnAbilityStart() {}


    //Take away the ability
    protected virtual void OnDestroy() {}
}
