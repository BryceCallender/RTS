using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIn : BaseHarvesterAIFSM 
{
    public GameObject[] resourceCollectors;
    public GameObject nearestResourceCollector;

    public float timer;
    public float timeToWait = 1.0f;

    private AI aiScript;

	//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        resourceCollectors = GameObject.FindGameObjectsWithTag("collector");
        aiScript = FindObjectOfType<AI>();
        timer = 0;

        float distance = Mathf.Infinity;

        foreach (GameObject collector in resourceCollectors)
        {
            if (collector != null)
            {
                animator.SetBool("isCollectorExisiting",true);
                float collectorDistance = Vector3.Distance(harvester.transform.position,collector.transform.position);
                if (nearestResourceCollector == null || collectorDistance < distance)
                {
                    nearestResourceCollector = collector;
                    distance = collectorDistance;
                }
            }
        }

        if (nearestResourceCollector == null)
        {
            return;
        }
	}

	//OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        agent.destination = nearestResourceCollector.transform.position;
        agent.stoppingDistance = 3;
        timer += Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance && timer >= timeToWait)
        {
            timer = 0;
            aiScript.currency += harvesterScript.resourceAmount;
            harvesterScript.resourceAmount = 0;
            animator.SetBool("isTurnedIn",true);
        }

	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isFull",false);
        animator.SetBool("isTurnedIn",false);
        animator.SetBool("isResourceGone",false);
    }
}
