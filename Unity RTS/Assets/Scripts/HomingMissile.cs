using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [Header("Missle Enemy:")]
    public GameObject enemy;

    [Header("Missle Vectors:")]
    public Vector3 myPosition;
    public Vector3 enemyPosition;
    public Vector3 lookDir;

    //[Header("Missle Properties")]
    //public float turnSpeed;
    float speed;

    // Use this for initialization
    void Start()
    {
        myPosition = transform.position;
        enemyPosition = enemy.transform.position;
        lookDir = Vector3.zero;
        speed = gameObject.GetComponent<HyperbitProjectileScript>().speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy != null)
        {
            myPosition = transform.position;
            enemyPosition = enemy.transform.position;

            transform.LookAt(enemyPosition);

            //Move towards the enemy slowly
            transform.position = Vector3.MoveTowards(myPosition, enemyPosition, speed * Time.deltaTime);
        }
    }
}