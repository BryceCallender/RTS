using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindResource : BaseHarvesterAIFSM
{
    public float timer;
    public float timeToWait = 1.0f;

	//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        base.OnStateEnter(animator, stateInfo,layerIndex);
        timer = 0;
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if(harvesterScript.resourceList.Count > 0)
        {
            agent.destination = harvesterScript.nearestResource.transform.position;
            agent.stoppingDistance = 3;
            timer += Time.deltaTime;
            if (agent.remainingDistance <= agent.stoppingDistance && timer >= timeToWait)
            {
                timer = 0;
                animator.SetBool("isAtResource", true);
            }
        }
        else
        {
            animator.SetBool("foundResource",false);   
        }

	}
}
