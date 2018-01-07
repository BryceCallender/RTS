using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
    public List<GameObject> targets;
    public GameObject bullet;
    public Transform turretEnd;
    public Transform turretRotator;
	public float health = 100f;
    public int range = 15;
    public int team;
    public int layerTeam;

	public HyperbitProjectileScript hyperProjectileScript;

	// Use this for initialization
	void Start () 
    {
        layerTeam = this.gameObject.layer;
        switch(layerTeam)
        {
            case 8: team = 0;
                break;
            case 9: team = 1;
                break;
        }
        targets = new List<GameObject>();
	}

	private void Die()
	{
		Destroy(gameObject);
	}

	private void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		hyperProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

		if(hyperProjectileScript.team.Equals(team))
		{
			return;
		}

		if (!hyperProjectileScript.owner.Contains("Red")
			&& !hyperProjectileScript.team.Equals(team))
		{
			//Physics.IgnoreLayerCollision(8, 10, false);
			if (collision.gameObject.tag.Contains("Laser")
				&& collision.gameObject.layer == 10)
			{
				TakeDamage(5);
			}
			else if (collision.gameObject.tag.Contains("Cluster")
					 && collision.gameObject.layer == 10)
			{
				TakeDamage(10);
			}
		}

	}

}
