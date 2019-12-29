using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUnit : Unit
{
    protected Animator animator;

    public bool isMoving;
    public bool isAttacking;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if(agent.velocity != Vector3.zero)
        {
            isMoving = true;
            isAttacking = false;
        }
        else
        {
            isMoving = false;
            isAttacking = nearestEnemy != null; //if we have an enemy selected
        }

        if (isAttacking)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * turnSpeed);
        }

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAttacking", isAttacking);
    }
}
