using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : BaseHarvesterAIFSM 
{
    public float harvestTime = 2.0f;
    public float harvestCoolDown = 3.0f;
    public int harvestAmount = 10;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        harvestCoolDown -= Time.deltaTime;

        if(harvesterScript.nearestResource.resourceLeft <= 0)
        {
            animator.SetBool("isResourceGone",true);
            harvesterScript.resourceList.Remove(harvesterScript.nearestResource);
            return;
        }

        if(harvesterScript.resourceAmount == harvesterScript.maxResourceToCollect)
        {
            animator.SetBool("isFull",true);
            return;
        }

        if (harvestCoolDown < 0)
        {
            harvestCoolDown = harvestTime;
            harvesterScript.resourceAmount += harvestAmount;
            harvesterScript.nearestResource.resourceLeft -= harvestAmount;
        }
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAtResource", false);
    }
}
