using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindClick : MonoBehaviour
{
	//public Transform target;
	public Vector3 targetPosition;

    private NavMeshAgent agent;

    //Raycast to deal with where we clicked
    RaycastHit hitInfo;


    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void LateUpdate()
    {
        if(this.GetComponent<UnitSelected>().selected)
        {
			// If we are not in range, become an agent again
			agent.enabled = true;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
			{
				if (Input.GetMouseButtonDown(1) && Mouse.IsDragging  == false && hitInfo.collider.gameObject.layer != 9)
				{
					targetPosition = hitInfo.point;
					//transform.LookAt(targetPosition);
					agent.destination = targetPosition;

				}
			}
        }
    }
}
