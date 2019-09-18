using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechUnit : Unit
{
    [SerializeField]
    private Animator animator;

    private bool isMoving;
    private bool isAttacking;
    private bool isPatrol;
    
    
    protected override void Fire()
    {
        base.Fire();
    }
    
    
}
