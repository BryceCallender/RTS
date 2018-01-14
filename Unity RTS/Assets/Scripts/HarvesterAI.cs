using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class HarvesterAI : MonoBehaviour 
{
    public int health = 30;
    public int resourceAmount = 0;
    public int maxResourceToCollect = 100;
	public int team = 1;

    public Resource[] resources;
    public Resource nearestResource;
    public List<Resource> resourceList;

    public Animator anim;
	public Slider healthBar;

	private HyperbitProjectileScript hyperbitProjectileScript;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
		healthBar.gameObject.SetActive(false);
		healthBar.maxValue = health;
		healthBar.value = health;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //TODO::fix this. This is gross making new lists everytime maybe dont 
        //need it since all resources will be laid out in game??? if so 
        //place back in start and no more worrying.
        resources = FindObjectsOfType<Resource>();
        resourceList = new List<Resource>(resources);

        FindResource();
        if(resourceList.Count > 0)
            anim.SetBool("foundResource",true);
	}

    public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach(Resource resource in resources)
        {
            if(resource != null)
            {
                float resourceDistance = Vector3.Distance(this.transform.position, resource.transform.position);
                if (nearestResource == null || resourceDistance < distance)
                {
                    nearestResource = resource;
                    distance = resourceDistance;
                }
            }
        }

        if (nearestResource == null)
            return;
    }

	public void Die()
	{
		Destroy(gameObject);
	}


	public void TakeDamage(int damage)
	{
		healthBar.gameObject.SetActive(true);
		health -= damage;
		healthBar.value -= damage;
		if (health <= 0)
		{
			Die();
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		hyperbitProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

		if (hyperbitProjectileScript.team.Equals(team))
		{
			return;
		}

		if (!hyperbitProjectileScript.owner.Contains("Red")
			&& !hyperbitProjectileScript.team.Equals(team))
		{
			//Physics.IgnoreLayerCollision(9, 10, false);
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
