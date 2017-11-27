using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBuilding : MonoBehaviour 
{
    public int health;
    private int team = 0;

	// Use this for initialization
	void Start () 
    {
        health = 300;
	}
	
    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(8, 10);
            //Debug.Log("Same team bro");
        }

        if (!collision.gameObject.GetComponent<HyperbitProjectileScript>().owner.Contains("Blue")
            && !collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(9, 10, false);
            if (collision.gameObject.tag.Contains("Laser") && collision.gameObject.layer == 10)
            {
                TakeDamage(5);
            }
            else if (collision.gameObject.tag.Contains("Cluster") && collision.gameObject.layer == 10)
            {
                TakeDamage(10);
            }

        }
    }
}
