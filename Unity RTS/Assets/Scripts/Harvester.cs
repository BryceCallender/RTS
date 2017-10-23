using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Harvester : MonoBehaviour
{
    public int health = 100;
    public int resourceAmount = 0;
    public int maxResourceToCollect = 100;
    public int cost = 5;
    public Resource[] resources;
    public Resource nearestResource;
    public Transform resourceCollector;

    static GameController gameController;

    private NavMeshAgent agent;
    private float harvestTime = 2.0f;
    private float harvestCoolDown = 3.0f;
    private int harvestAmount = 10;

    private bool isTargetingResource = false;
    private bool isFull = false;
    private bool isTurnedIn = false;
    private bool isAtResource = false;


    private float timer = 0;
    private float timeToWait = 1.0f;

    private RaycastHit hitInfo;
    private UnitSelected unitSelected;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        resources = FindObjectsOfType<Resource>();
        gameController = FindObjectOfType<GameController>();
        unitSelected = GetComponent<UnitSelected>();
        agent.stoppingDistance = 3;
        if(resourceCollector == null)
        {
            resourceCollector = transform.Find("SupplyBuilding");
        }

    }

    private void Update()
    {
        FindResource();
        if(nearestResource != null)
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray2, out hitInfo, Mathf.Infinity))
            {
                WhereToGo();
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !isFull && isAtResource)
            {
                CollectResource();
            }

            if (resourceAmount >= maxResourceToCollect || nearestResource.resourceLeft == 0)
			{
				isFull = true;
                if(resourceCollector != null)
                {
                    agent.destination = resourceCollector.position;
                }
                timer += Time.deltaTime;
                if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                {
                    
                    isTurnedIn = true;
                    TurnInResource();
                    timer = 0;
                }
                isTurnedIn = false;
                isFull = false;
			}

        }
        else
        {
            //raycast to check where user clicked mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //TODO: Need to make harvester more effective with moving 
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                //If right clicked and unit is selected go to resource
                WhereToGo();
                if (nearestResource == null && resourceAmount > 0)
                {
                    if(resourceCollector != null)
                    {
                        agent.destination = resourceCollector.position;
                    }

                    timer += Time.deltaTime;
                    if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                    {
                        Debug.Log("We should turn in the stoff");
                        TurnInResource();
                        timer = 0;
                        isTurnedIn = true;
                    }
                    isTurnedIn = false;
                    isFull = false;
                }
            }
        }
    }

    /*
     * This function makes the harvester wait a bit to collect each resource 
     * and as it collects it takes from the resources resovoir 
     */
    public void CollectResource()
    {
        harvestCoolDown -= Time.deltaTime;
        if(harvestCoolDown < 0)
        {
            harvestCoolDown = harvestTime;
            resourceAmount += harvestAmount;
            nearestResource.resourceLeft -= harvestAmount;
        }
    }

	/*
     * This function makes the harvester turn in what it has and adds 
     * to the currency of the user 
     */
	public void TurnInResource()
    {
        gameController.currency += resourceAmount;
        resourceAmount = 0;
        
    }

	/*
     * This function looks at all the resources on map and picks closest one
     */
	public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach (Resource resource in resources)
        {
            if(resource != null)
            {
				float resourceDistance = Vector3.Distance(this.transform.position, resource.transform.position);
				if (nearestResource == null || resourceDistance < distance)
				{
					//nearestResource = resource;
					//distance = resourceDistance;
				}
            }

        }

        if (nearestResource == null)
        {
            //return; 
        }
    }

    /*
     * Tells the harvester where to go based on if it is selected and if 
     * the user has right clicked down on  the mouse. If it hits a resource the
     * harvester will go to the resource and start collecting and if its just
     * a random point on the map the harvester will go to that specific point
     */
    public void WhereToGo()
    {
		if (Input.GetMouseButtonDown(1) && unitSelected.selected)
		{
			if (hitInfo.transform.gameObject.CompareTag("Resource"))
			{
				Debug.Log("hit resource after we had resource");
				GameObject resource = hitInfo.transform.gameObject;
				nearestResource = resource.GetComponent<Resource>();
				agent.destination = nearestResource.transform.position;
                isAtResource = true;
			}
			else
			{
				agent.destination = hitInfo.point;
                isAtResource = false;
			}

		}
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
        }
        else
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
