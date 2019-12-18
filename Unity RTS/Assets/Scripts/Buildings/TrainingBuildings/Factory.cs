using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : TrainingBuilding
{
    public Animation leftDoorAnimation;
    public Animation rightDoorAnimation;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            AnimateBuildingInProductionFinish();
        }
    }

    protected override void AnimateBuildingDuringProduction()
    {

    }

    protected override void AnimateBuildingInProductionFinish()
    {
        
    }
}
