using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleAttacking : StateMachineBehaviour 
{
    //Missle Necessities
	public MissleTurret turret;
	public GameObject enemy;
	public Vector3 direction;

    //Timers
	private float timeToFire = 5.0f;
	private float fireCoolDown = 5.0f;
    private float TIME_TO_RESTORE_MISSLE_HEAD = 3.0f;

    //Booleans
	private bool hasFired = false;
	private bool missleIsTraveling = false;

    HyperbitProjectileScript hyperProjectileScript;
    HomingMissile missile;

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

			turret.turretRotator.rotation = Quaternion.Lerp(Quaternion.Euler(direction), 
                                                            animator.gameObject.GetComponent<Transform>().rotation, 
                                                            1.0f);
		}

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
        int turrentNum = Random.Range(0, turret.missleRepresentations.Count);
		timeToFire += Time.deltaTime;
        MissleHead missileHead = turret.missleRepresentations[turrentNum];

		if (timeToFire >= fireCoolDown && direction.magnitude <= turret.range)
		{
            projectile = Instantiate(turret.bullet, turret.missleRepresentations[turrentNum].turretEnds.position, 
                                     turret.missleRepresentations[turrentNum].turretEnds.rotation);
            missile = projectile.GetComponent<HomingMissile>();
            hyperProjectileScript = projectile.GetComponent<HyperbitProjectileScript>();
            missile.enemy = enemy;
			HideMissle(turrentNum);
			hasFired = true;
            timeToFire = 0;
		}

        if(hasFired && projectile != null)
		{
			missleIsTraveling = true;
			projectile.tag = "Missle";
            hyperProjectileScript.owner = animator.gameObject.name;
            hyperProjectileScript.team = turret.team;
            int speed = hyperProjectileScript.speed;
			projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
		}

		if(!missleIsTraveling)
		{
			timeToFire = 0;
		}
	}

	public void HideMissle(int missleNum)
	{
        MissleHead missileHead = turret.missleRepresentations[missleNum];
        missileHead.turretEnds.gameObject.SetActive(false);
        missileHead.hasBeenFired = true;
        if (missileHead.hasBeenFired)
        {
            //Starts A coroutine to make game wait 3 seconds to reload before
            //shooting another missile
            turret.StartCoroutine(StartResetTimer(missileHead,missleNum));
        }
	}

	public void ResetMissle(int missleNum)
	{
        turret.missleRepresentations[missleNum].turretEnds.gameObject.SetActive(true);
	}

    public IEnumerator StartResetTimer(MissleHead missileHead,int missleNum)
    {
        yield return new WaitForSeconds(TIME_TO_RESTORE_MISSLE_HEAD);
        missileHead.turretEnds.gameObject.SetActive(true); 
    }
}