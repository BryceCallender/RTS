using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : Building
{
    public int capacityIncrease = 10;

    protected override void Update()
    {
        base.Update();

        if(IsBuildingAvailableToUse())
        {
            //Access the player and add 10 to the capacity
        }
    }

    private void OnDestroy()
    {
        //Access player and remove the 10 from the capacity
    }
}
