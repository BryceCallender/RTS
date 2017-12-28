using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour 
{
    public Turret turret;
    public GameObject enemy;
    public Vector3 direction;

    private float timeToFire = 3.0f;
    private float fireCoolDown = 3.0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turret = animator.gameObject.GetComponent<Turret>();
        if(turret.targets.Count > 0)
        {
            enemy = turret.targets[0];
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if(turret.targets.Count > 0)
        {
            if(enemy != null)
            {
                LockOn();
                Fire(animator);
            }
                
        }

        if (enemy == null || direction.magnitude > turret.range)
        {
            if(enemy == null)
            {
                turret.targets.RemoveAt(0);
                if(turret.targets.Count > 0)
                    enemy = turret.targets[0];
            }

            turret.turretRotator.rotation = Quaternion.Lerp(Quaternion.Euler(direction), animator.gameObject.GetComponent<Transform>().rotation, 1.0f);
        }
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
        GameObject projectile;
        timeToFire += Time.deltaTime;
        if (timeToFire >= fireCoolDown && direction.magnitude <= turret.range)
        {
            timeToFire = 0;
            projectile = (GameObject)Instantiate(turret.bullet, turret.turretEnd.transform.position, turret.turretEnd.transform.rotation);
            projectile.tag = "Laser";
            projectile.GetComponent<HyperbitProjectileScript>().owner = animator.gameObject.name;
            projectile.GetComponent<HyperbitProjectileScript>().team = turret.team;
            //projectile.transform.LookAt(nearestEnemy.transform.position);
            int speed = projectile.GetComponent<HyperbitProjectileScript>().speed;
            projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
        }
    }
}
