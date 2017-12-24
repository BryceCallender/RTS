using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseHarvesterAIFSM : StateMachineBehaviour 
{
    public GameObject harvester;
    public NavMeshAgent agent;
    public HarvesterAI harvesterScript;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        harvester = animator.gameObject;
        agent = harvester.GetComponent<NavMeshAgent>();
        harvesterScript = harvester.GetComponent<HarvesterAI>();
    }
}
