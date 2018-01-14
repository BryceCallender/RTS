using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleAttacking : StateMachineBehaviour 
{
	public MissleTurret turret;
	public GameObject enemy;
	public Vector3 direction;

	private float timeToFire = 3.0f;
	private float fireCoolDown = 3.0f;
	private bool hasFired = false;
	private bool missleIsTraveling = false;

	//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		turret = animator.gameObject.GetComponent<MissleTurret>();
		if (turret.targets.Count > 0)
		{
			enemy = turret.targets[0];
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (turret.targets.Count > 0)
		{
			if (enemy != null)
			{
				LockOn();
				Fire(animator);
			}

		}

		if (enemy == null || direction.magnitude > turret.range)
		{
			if (enemy == null)
			{
				enemy = ResetTarget();
			}

			turret.turretRotator.rotation = Quaternion.Lerp(Quaternion.Euler(direction), animator.gameObject.GetComponent<Transform>().rotation, 1.0f);
		}

		//TODO::fix
		if (turret.targets.Count > 0)
		{
			if (Vector3.Distance(animator.transform.position, enemy.transform.position) > turret.range)
			{
				enemy = ResetTarget();
			}
		}

	}

	private GameObject ResetTarget()
	{
		GameObject newTarget = null;

		if (turret.targets.Count == 0)
		{
			return newTarget;
		}

		if (turret.targets.Count > 1)
		{
			turret.targets.Remove(enemy);
			newTarget = turret.targets[0];
		}
		else
		{
			turret.targets.Remove(enemy);
		}

		return newTarget;
	}

	public void LockOn()
	{
		direction = enemy.transform.position - turret.gameObject.transform.position;
		// Debug.DrawRay(turret.turretEnd.position,direction,Color.red,Mathf.Infinity);

		if (direction.magnitude <= turret.range)
		{
			if (direction != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				turret.turretRotator.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
			}
		}
	}

	public void Fire(Animator animator)
	{
		GameObject projectile = null;
		int turrentNum = Random.Range(0, turret.turretEnds.Length);
		timeToFire += Time.deltaTime;

		if (timeToFire >= fireCoolDown && direction.magnitude <= turret.range)
		{
			projectile = (GameObject)Instantiate(turret.bullet, turret.turretEnds[turrentNum].transform.position, turret.turretEnds[turrentNum].transform.rotation);
			HideMissle(turrentNum);
			hasFired = true;
		}

		if(hasFired)
		{
			missleIsTraveling = true;
			projectile.tag = "Missle";
			projectile.GetComponent<HyperbitProjectileScript>().owner = animator.gameObject.name;
			projectile.GetComponent<HyperbitProjectileScript>().team = turret.team;
			//projectile.transform.LookAt(nearestEnemy.transform.position);
			int speed = projectile.GetComponent<HyperbitProjectileScript>().speed;

			float rotateAmount = Vector3.Cross(direction,Vector3.forward).y;
			//projectile.GetComponent<Rigidbody>().angularVelocity = 
			projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
		}

		if(!missleIsTraveling)
		{
			timeToFire = 0;
		}
		
	}

	public void HideMissle(int missleNum)
	{
		turret.turretEnds[missleNum].gameObject.SetActive(false);
	}

	public void ResetMissle(int missleNum)
	{
		turret.turretEnds[missleNum].gameObject.SetActive(true);
	}
}
