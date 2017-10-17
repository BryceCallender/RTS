using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    public float damage = 1f;
    public float speed = 15f;
    public Transform target;

    // Use this for initialization
    void Start () 
    {
        
    }
    
    // Update is called once per frame
    void Update () 
    {
        if (target == null) 
        {
            //Deletes bullets
            Destroy (gameObject);
            return;
        }

        Vector3 direction = target.transform.position - this.transform.localPosition;
        float distance = speed * Time.deltaTime;
        if (direction.magnitude <= distance) 
        {
            HitEnemy ();
        }
        else 
        {
            // TODO: Consider ways to smooth this motion.

            // Move towards node
            transform.Translate( direction.normalized * distance, Space.World );
            Quaternion targetRotation = Quaternion.LookRotation( direction );
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);
        }


    }

    void HitEnemy()
    {
        //Destroys bullet so it doesnt continually do damage
        Destroy(gameObject);
    }
}