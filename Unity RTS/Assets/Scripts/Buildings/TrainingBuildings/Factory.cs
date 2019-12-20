using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : TrainingBuilding
{
    public Animator[] animators;

    protected override void Start()
    {
        base.Start();

        animators = GetComponentsInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.F1))
        {
            AnimateBuildingInProductionFinish();
            Debug.Log("Playing animation");
        }
    }

    protected override void AnimateBuildingDuringProduction()
    {

    }

    protected override void AnimateBuildingInProductionFinish()
    {
        foreach (Animator animator in animators)
            animator.SetTrigger("finishedProduction");
    }
}
