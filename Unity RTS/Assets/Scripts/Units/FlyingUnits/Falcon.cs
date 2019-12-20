using UnityEngine;
using System.Collections;

public class Falcon : FlyingUnit
{
    private float rotationTime = 0.0f;

    protected override void Start()
    {
        base.Start();
        turnSpeed = 10;
    }

    protected override void AimTurrets()
    {
        base.AimTurrets();
        if(nearestEnemy != null)
        {
            Vector3 direction = transform.position - nearestEnemy.transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * turnSpeed);
        }
    }

}
