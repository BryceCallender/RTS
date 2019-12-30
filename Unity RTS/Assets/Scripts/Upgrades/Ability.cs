using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public float abilityDuration;

    private void Update()
    {
        if(abilityDuration <= 0)
        {
            Destroy(this);
        }

        abilityDuration -= Time.deltaTime;
    }

    //Perform the action
    protected virtual void OnAbilityStart() { }


    //Take away the ability
    protected virtual void OnDestroy() {}
}
