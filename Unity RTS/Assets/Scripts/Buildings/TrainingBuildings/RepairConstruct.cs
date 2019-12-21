using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairConstruct : TrainingBuilding
{
    private float repairRate = 1.0f;




    protected override void Update()
    {
        base.Update();

        //Check for repairs
    }


    private void RepairUnit(GameObject unit)
    {
        unit.GetComponent<Health>().Repair(repairRate * Time.deltaTime);
    }
}
