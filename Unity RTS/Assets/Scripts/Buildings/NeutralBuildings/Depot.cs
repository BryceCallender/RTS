using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : Building
{
    public int capacityIncrease = 10;
    private bool hasAppliedCapacity = false;

    protected override void Update()
    {
        base.Update();

        if(IsBuildingAvailableToUse() && !hasAppliedCapacity)
        {
            //Access the player and add 10 to the capacity
            GameController.Instance.GetPlayer().ChangeCapacity(capacityIncrease);
            hasAppliedCapacity = true;
        }
    }

    private void OnDestroy()
    {
        //Access player and remove the 10 from the capacity
        GameController.Instance.GetPlayer().ChangeCapacity(-capacityIncrease);
    }
}
