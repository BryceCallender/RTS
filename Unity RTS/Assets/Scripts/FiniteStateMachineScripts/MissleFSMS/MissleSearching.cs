﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleSearching : StateMachineBehaviour 
{
	public MissleTurret turret;
	public bool isRotating;

	public float turnDelay = 10.0f;

	private Building myBuilding;

	//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		isRotating = false;
		turret = animator.gameObject.GetComponent<MissleTurret>();
	}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (turret.targets.Count.Equals(0))
        {
            //Rotate pls 
            //turret.StartCoroutine(Rotate());
            SearchForEnemies();
        }
        else
        {
            animator.SetBool("foundEnemy", true);
        }
    }

	public void SearchForEnemies()
	{
		turret.targets.Clear();
		RaycastHit[] hitInfo;
		int layerMask;

		if (turret.team.Equals(0))
		{
			layerMask = 1 << 9;
		}
		else
		{
			layerMask = 1 << 8;
		}

		hitInfo = Physics.SphereCastAll(turret.transform.position, turret.range, Vector3.forward, Mathf.Infinity, layerMask);

		for (int i = 0; i < hitInfo.Length; i++)
		{
			turret.targets.Add(hitInfo[i].collider.gameObject);
		}
	}
}
