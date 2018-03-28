using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

[System.Serializable]
public struct MissleHead
{
    public bool hasBeenFired;
    public Transform turretEnds;
}

public class MissleTurret : MonoBehaviour
{
	public List<GameObject> targets;
    public List<MissleHead> missleRepresentations;
	public GameObject bullet;
	public Transform turretRotator;
	public float health = 100f;
	public int range = 15;
	public int team;
	public int layerTeam;

	public HyperbitProjectileScript hyperProjectileScript;

	// Use this for initialization
	void Start()
	{
		//Since its turrents the only targets is the units so layers
		layerTeam = this.gameObject.layer;
		switch (layerTeam)
		{
			case 8:
                team = (int)Team.BLUE;
				break;
			case 9:
                team = (int)Team.RED;
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

	private void OnTriggerEnter(Collider collision)
	{
		hyperProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

		if (hyperProjectileScript.team.Equals(team))
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
