using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour 
{
	public Transform target;
	public Vector3 targetPosition;

    private NavMeshAgent agent;

	public void Start()
	{
		targetPosition = target.transform.position;
		//Get a reference to the Seeker component we added earlier
        agent = GetComponent<NavMeshAgent>();

	}


	public void FixedUpdate()
	{
        //Sets destination 
        agent.destination = target.position;
	}
}
